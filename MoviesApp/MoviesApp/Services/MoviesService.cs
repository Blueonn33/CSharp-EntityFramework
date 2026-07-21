using MoviesApp.Models;
using MoviesApp.Services.Interfaces;
using MoviesApp.ViewModels.Movies;

namespace MoviesApp.Services
{
    public class MoviesService : IMoviesService
    {
        public async Task AddAsync(Movie movie)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AllMoviesIndexViewModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Movie?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Movie movie)
        {
            throw new NotImplementedException();
        }
    }
}