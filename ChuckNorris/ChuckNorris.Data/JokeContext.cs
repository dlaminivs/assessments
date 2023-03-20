using ChuckNorris.Models;
using Microsoft.EntityFrameworkCore;


namespace ChuckNorris.Data
{
    public class JokeContext : ChuckNorrisContext
    {
        public JokeContext(DbContextOptions<JokeContext> options) : base(options)
        {
        }

        public DbSet<Joke> Jokes { get; set; }
        public DbSet<UserFavouriteJoke> UserFavouriteJokes { get; set; }
    }

}
