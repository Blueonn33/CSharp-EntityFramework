using System.ComponentModel.DataAnnotations;

namespace MoviesApp.DTOs.Json
{
    public class ImportJsonMovieDto
    {
        [Range(1, int.MaxValue)]
        public int Id
        {
            get; set;
        }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        []
        public string Genre { get; set; } = null!;

        public string ReleaseDate { get; set; } = null!;

        public string Director { get; set; } = null!;

        public int Duration { get; set; }

        public string Description { get; set; } = null!;

        public string? ImageUrl { get; set; }
    }
}