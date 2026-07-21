using MoviesApp.Models;
using MoviesApp.ViewModels.Movies;

namespace MoviesApp.Services.Interfaces
{
    public interface IMoviesService
    {
        Task<IEnumerable<AllMoviesIndexViewModel>> GetAllAsync();

        Task<Movie?> GetByIdAsync(int id);

        Task AddAsync(Movie movie);

        Task UpdateAsync(Movie movie);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
    }
}
