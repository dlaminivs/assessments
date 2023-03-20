using ChuckNorris.Models;

namespace ChuckNorris.Service
{
    public interface IJokeService
    {
        Task<Joke?> GetRandomJokeAsync();
        Task<Joke?> GetJokeByIdAsync(string Id);
    }
}
