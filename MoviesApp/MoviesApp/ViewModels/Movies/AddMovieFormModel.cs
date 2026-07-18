using System.ComponentModel.DataAnnotations;

namespace MoviesApp.ViewModels.Movies
{
    public class AddMovieFormModel
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Genre { get; set; } = null!;

        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Director { get; set; } = null!;

        [Range(1, 500)]
        public int Duration
        {
            get; set;
        }

        [Required]
        public DateTime ReleaseDate
        {
            get; set;
        }

        [Required]
        [MinLength(10)]
        [MaxLength(2000)]
        public string Description { get; set; } = null!;

        [MaxLength(2048)]
        public string? ImageUrl
        {
            get; set;
        }
    }
}
