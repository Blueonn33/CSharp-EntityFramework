using System.ComponentModel.DataAnnotations;
using static MoviesApp.Common.EntityValidation;

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
        [MaxLength(MovieTitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MinLength(MovieGenreMinLength)]
        [MaxLength(MovieGenreMaxLength)]
        public string Genre { get; set; } = null!;

        [Required]
        [RegularExpression(MovieReleaseDateRegexPattern)]
        public string ReleaseDate { get; set; } = null!;

        [Required]
        [MinLength(MovieDirectorMinLength)]
        [MaxLength(MovieDirectorMaxLength)]
        public string Director { get; set; } = null!;

        [Range(MovieDurationMinValue, MovieDurationMaxValue)]
        public int Duration
        {
            get; set;
        }

        [Required]
        [MinLength(MovieDescriptionMinLength)]
        [MaxLength(MovieDescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [MaxLength(MovieImageUrlMaxLength)]
        public string? ImageUrl
        {
            get; set;
        }
    }
}