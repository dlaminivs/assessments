using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChuckNorris.Models
{
    public class Joke : EntityBase
    {
        [NotMapped]
        public List<string>? Categories { get; set; }
        public DateTime? Created_at { get; set; }
        public string? Icon_url { get; set; }
        public DateTime? Updated_at { get; set; }
        public string? Url { get; set; }
        public string? Value { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}
