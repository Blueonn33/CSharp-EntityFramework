using MoviesApp.DTOs.Movie;
using MoviesApp.Models;

namespace MoviesApp.Services.Interfaces
{
    public interface IMoviesService
    {
        Task<IEnumerable<GetAllMoviesDto>> GetAllAsync(int pageNumber = 1, int pageSize = 25);

        Task<Movie?> GetByIdAsync(int id);

        Task AddAsync(Movie movie);

        Task UpdateAsync(Movie movie);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
    }
}
