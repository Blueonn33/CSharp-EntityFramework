using MiniCinemaApp.Data.Models;

namespace MoviesApp.Services.Interfaces
{
    public interface IWatchlistService
    {
        Task<IEnumerable<Movie>> GetAllAsync();

        Task AddAsync(int movieId);

        Task RemoveAsync(int movieId);
    }
}
