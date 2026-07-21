using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.DTOs.Movie;
using MoviesApp.Models;
using MoviesApp.Services.Interfaces;

namespace MoviesApp.Services
{
    public class MoviesService : IMoviesService
    {
        private readonly MoviesAppDbContext _dbContext;

        public MoviesService(MoviesAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

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

        public async Task<IEnumerable<GetAllMoviesDto>> GetAllAsync(int pageNumber = 1, int pageSize = 25)
        {
            IEnumerable<GetAllMoviesDto> allMovies = await _dbContext.Movies
                .AsNoTracking()
                .OrderBy(m => m.Title)
                .ThenByDescending(m => m.ReleaseDate)
                .Select(m => new GetAllMoviesDto()
                {
                    Id = m.Id,
                    Title = m.Title,
                    Genre = m.Genre,
                    Director = m.Director,
                    ReleaseDate = DateOnly.FromDateTime(m.ReleaseDate),
                    Description = m.Description,
                    Duration = m.Duration,
                    ImageUrl = m.ImageUrl,
                    IsAddedInWatchlist = _dbContext.Watchlists.
                        Any(w => w.MovieId == m.Id)
                })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();

            return allMovies;
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