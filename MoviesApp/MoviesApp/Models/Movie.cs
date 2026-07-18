using System.ComponentModel.DataAnnotations;

namespace MoviesApp.Models
{
    public class Movie
    {
        [Key]
        public int Id
        {
            get; set;
        }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Genre { get; set; } = null!;

        public DateTime ReleaseDate
        {
            get; set;
        }

        [Required]
        [MaxLength(100)]
        public string Director { get; set; } = null!;

        public int Duration
        {
            get; set;
        }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; } = null!;

        [MaxLength(2000)]
        public string? ImageUrl
        {
            get; set;
        }
    }
}
