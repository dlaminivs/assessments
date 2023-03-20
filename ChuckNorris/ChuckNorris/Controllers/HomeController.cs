using ChuckNorris.Models;
using ChuckNorris.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace ChuckNorris.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IJokeService _jokeService;
        private readonly IFavouriteJokeService _favouriteJokeService;

        public HomeController(ILogger<HomeController> logger, IJokeService jokeService, IFavouriteJokeService favouriteJokeService)
        {
            _logger = logger;
            _jokeService = jokeService;
            _favouriteJokeService = favouriteJokeService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> IndexAsync(int page = 1, string jokeid = "")
        {
            JokeViewModel jokeViewModel = new JokeViewModel
            {
                CurrentPage = page,
                JokesPerPage = 3
            };


            if (string.IsNullOrEmpty(jokeid))
            {
                var randomJoke = await _jokeService.GetRandomJokeAsync();
                AddJokeToViewModel(jokeViewModel, randomJoke);
            }
            else
            {
                var randomJoke = await _jokeService.GetJokeByIdAsync(jokeid);
                AddJokeToViewModel(jokeViewModel, randomJoke);
            }

            await AddFavouritesToViewModel(jokeViewModel);

            return View(jokeViewModel);
        }


        public async Task<IActionResult> SaveFavourite(string jokeId, int page = 1)
        {
            var userId = GetCurrentUserId();
            JokeViewModel jokeViewModel = new JokeViewModel
            {
                CurrentPage = page,
                JokesPerPage = 3
            };
            if (jokeId != null && userId != null)
            {
                var updatedJoke = await _favouriteJokeService.SaveFavouriteAsync(jokeId, userId);
                AddJokeToViewModel(jokeViewModel, updatedJoke?.Joke);
                await AddFavouritesToViewModel(jokeViewModel);
            }
            return View("Index", jokeViewModel);
        }

        public async Task<IActionResult> RemoveFavourite(string jokeid, string favId, int page = 1)
        {


            var userId = GetCurrentUserId();
            JokeViewModel jokeViewModel = new()
            {
                CurrentPage = page,
                JokesPerPage = 3,
            };

            if (jokeid != null && userId != null)
            {
                var deleted = jokeid == favId ? await _favouriteJokeService.DeleteFavouriteJokeByJokeIdAsync(jokeid)
                                            : await _favouriteJokeService.DeleteFavouriteJokeByIdAsync(favId);
                if (deleted)
                {
                    var randomJoke = await _jokeService.GetJokeByIdAsync(jokeid);
                    AddJokeToViewModel(jokeViewModel, randomJoke);
                    await AddFavouritesToViewModel(jokeViewModel);
                }

            }
            return View("Index", jokeViewModel);
        }

        private string? GetCurrentUserId()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var userIdClaim = claimsIdentity.Claims
                    .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (userIdClaim != null)
                {
                    return userIdClaim.Value;
                }
            }

            return null;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task AddFavouritesToViewModel(JokeViewModel jokeViewModel)
        {
            var userId = GetCurrentUserId();
            if (!string.IsNullOrEmpty(userId))
            {
                var favouriteJokes = await _favouriteJokeService.GetAllFavouriteJokesAsync(userId);
                if (favouriteJokes != null)
                {
                    jokeViewModel.FavouriteJokes = favouriteJokes;
                    jokeViewModel.IsFavourite = favouriteJokes.Any(a => a.Joke.Id == jokeViewModel.Joke.Id);
                }
            }
        }

        private static void AddJokeToViewModel(JokeViewModel jokeViewModel, Joke? randomJoke)
        {
            if (randomJoke != null)
            {
                jokeViewModel.Joke = randomJoke;
            }
        }

    }
}