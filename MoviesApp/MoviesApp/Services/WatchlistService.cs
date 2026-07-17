using MiniCinemaApp.Data.Models;
using MoviesApp.Services.Interfaces;

namespace MoviesApp.Services
{
    public class WatchlistService : IWatchlistService
    {
        public Task AddAsync(int movieId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Movie>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(int movieId)
        {
            throw new NotImplementedException();
        }
    }
}
