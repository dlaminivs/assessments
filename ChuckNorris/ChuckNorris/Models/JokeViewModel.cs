namespace ChuckNorris.Models
{
    public class JokeViewModel
    {
        public Joke? Joke { get; set; }
        public bool IsFavourite { get; set; }
        public List<UserFavouriteJoke>? FavouriteJokes { get; set; }
        public int JokesPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount()
        {
            return Convert.ToInt32(Math.Ceiling(FavouriteJokes.Count() / (double)JokesPerPage));
        }
        public IEnumerable<UserFavouriteJoke> PaginatedJokes()
        {
            int start = (CurrentPage - 1) * JokesPerPage;
            return FavouriteJokes.OrderByDescending(o => o.DateCreated).Skip(start).Take(JokesPerPage);
        }
    }
}
