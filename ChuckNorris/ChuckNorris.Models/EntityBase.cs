using System.ComponentModel.DataAnnotations;

namespace ChuckNorris.Models
{
    public class EntityBase
    {
        [Key]
        public string? Id { get; set; }
    }
}
