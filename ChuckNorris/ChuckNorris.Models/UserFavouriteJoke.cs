namespace ChuckNorris.Models
{
    public class UserFavouriteJoke : EntityBase
    {
        public string? UserId { get; set; }
        public Joke? Joke { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
