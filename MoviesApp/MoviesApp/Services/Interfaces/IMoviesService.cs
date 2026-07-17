using MiniCinemaApp.Data.Models;

namespace MoviesApp.Services.Interfaces
{
    public interface IMoviesService
    {
        Task<IEnumerable<Movie>> GetAllAsync();

        Task<Movie?> GetByIdAsync(int id);

        Task AddAsync(Movie movie);

        Task UpdateAsync(Movie movie);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);
    }
}
