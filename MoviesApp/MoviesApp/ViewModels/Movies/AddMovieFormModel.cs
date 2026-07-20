using System.ComponentModel.DataAnnotations;
using static MoviesApp.Common.EntityValidation;

namespace MoviesApp.ViewModels.Movies
{
    public class AddMovieFormModel
    {
        [Required]
        [MaxLength(MovieTitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MinLength(MovieGenreMinLength)]
        [MaxLength(MovieTitleMaxLength)]
        public string Genre { get; set; } = null!;

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
        [RegularExpression(MovieReleaseDateRegexPattern)]
        public DateTime ReleaseDate
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
