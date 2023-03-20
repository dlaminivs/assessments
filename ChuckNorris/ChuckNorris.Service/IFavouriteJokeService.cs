using ChuckNorris.Models;

namespace ChuckNorris.Service
{
    public interface IFavouriteJokeService
    {
        Task<UserFavouriteJoke?> SaveFavouriteAsync(string jokeId, string userId);

        Task<List<UserFavouriteJoke>> GetAllFavouriteJokesAsync(string userId);
        Task<bool> DeleteFavouriteJokeByIdAsync(string favjokeId);
        Task<bool> DeleteFavouriteJokeByJokeIdAsync(string jokeId);
    }
}
