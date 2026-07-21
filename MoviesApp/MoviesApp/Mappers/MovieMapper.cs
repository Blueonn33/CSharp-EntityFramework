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
    }
}
