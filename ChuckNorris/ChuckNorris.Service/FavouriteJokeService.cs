using ChuckNorris.Data.DataManager;
using ChuckNorris.Models;

namespace ChuckNorris.Service
{
    public class FavouriteJokeService : IFavouriteJokeService
    {
        private readonly IRepository<UserFavouriteJoke> _favouritesRepository;
        private readonly IRepository<Joke> _jokeRepository;
        public FavouriteJokeService(IRepository<UserFavouriteJoke> favrepository, IRepository<Joke> jokeRepository)
        {
            _favouritesRepository = favrepository;
            _jokeRepository = jokeRepository;
        }

        public async Task<bool> DeleteFavouriteJokeByIdAsync(string favjokeId)
        {
            var result = await _favouritesRepository.Delete(favjokeId);
            return result;
        }

        public async Task<bool> DeleteFavouriteJokeByJokeIdAsync(string jokeId)
        {
            bool isDeleted = false;
            var favJoke = await _favouritesRepository.Get(g => g.Joke.Id != null && g.Joke.Id == jokeId);
            if(favJoke != null)
            {
                isDeleted = await DeleteFavouriteJokeByIdAsync(favJoke?.FirstOrDefault()?.Id);
            }
            return isDeleted;
        }

        public async Task<List<UserFavouriteJoke>> GetAllFavouriteJokesAsync(string userId)
        {
            var favouriteJokes = await _favouritesRepository.Get(filter: (x => x.UserId == userId), includeProperties: "Joke");
            if (favouriteJokes == null)
            {
                return new List<UserFavouriteJoke>();
            }
            return favouriteJokes.ToList()!;
        }

        public async Task<UserFavouriteJoke?> SaveFavouriteAsync(string jokeId, string userId)
        {
            if (!string.IsNullOrEmpty(jokeId))
            {
                var joke = await _jokeRepository.GetByID(jokeId);
                if (joke != null)
                {
                    UserFavouriteJoke userFavouriteJoke = new UserFavouriteJoke
                    {
                        Id = Guid.NewGuid().ToString(),
                        Joke = joke,
                        UserId = userId
                    };
                    await _favouritesRepository.Insert(userFavouriteJoke);
                    return userFavouriteJoke;
                }
            }

            return null;
        }
    }
}
