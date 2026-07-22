using MoviesApp.DTOs.Movie;
using MoviesApp.ViewModels.Movies;
using Riok.Mapperly.Abstractions;

namespace MoviesApp.Mappers
{
    [Mapper]
    public partial class MovieMapper
    {
        [MapProperty(nameof(GetAllMoviesDto.ReleaseDate), nameof(AllMoviesIndexViewModel.ReleaseDate), StringFormat = "d")]
        public partial IEnumerable<AllMoviesIndexViewModel> MapToAllMoviesIndexViewModels(
            IEnumerable<GetAllMoviesDto> allMoviesDto);

        [MapProperty(nameof(AddMovieFormModel.ReleaseDate), nameof(AddNewMovieDto.ReleaseDate), Use = nameof(DateTimeToDateOnly))]
        public partial AddNewMovieDto MapToAddNewMovieDto(AddMovieFormModel model);

        protected DateOnly DateTimeToDateOnly(DateTime source)
        {
            return DateOnly.FromDateTime(source);
        }
    }
}
