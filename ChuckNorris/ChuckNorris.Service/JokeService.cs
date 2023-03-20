using ChuckNorris.Data.DataManager;
using ChuckNorris.Models;
using Newtonsoft.Json;

namespace ChuckNorris.Service
{
    public class JokeService : IJokeService
    {
        private readonly HttpClient _httpClient;
        private readonly IRepository<Joke> _jokeRepository;
        

        public JokeService(HttpClient httpClient, IRepository<Joke> jokeRepository)
        {
            _httpClient = httpClient;
            _jokeRepository = jokeRepository;
            
        }
        public async Task<Joke?> GetJokeByIdAsync(string Id)
        {
            return await _jokeRepository.GetByID(Id);
        }

        public async Task<Joke?> GetRandomJokeAsync()
        {
            var response = await _httpClient.GetAsync("https://api.chucknorris.io/jokes/random");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var joke = JsonConvert.DeserializeObject<Joke>(content);
            if (joke == null)
            {
                return null;
            }
            Joke insertedJoke = await _jokeRepository.Insert(joke);
            return insertedJoke;
        }



    }
}
