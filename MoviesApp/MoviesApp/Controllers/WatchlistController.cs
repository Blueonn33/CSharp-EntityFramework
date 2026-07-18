using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.Services.Interfaces;
using MoviesApp.ViewModels.Watchlist;

namespace MoviesApp.Controllers
{
    public class WatchlistController : Controller
    {
        // TODO: Extract the default image URL to a configuration file or a constant class
        private const string DefaultImageUrl =
            "https://img.freepik.com/free-vector/cinema-film-production-realistic-transparent-composition-with-isolated-image-clapper-with-empty-fields-vector-illustration_1284-66163.jpg?semt=ais_incoming&w=740&q=80";

        private readonly IMoviesService _moviesService;
        private readonly MoviesAppDbContext _dbContext;

        public WatchlistController(IMoviesService moviesService, MoviesAppDbContext dbContext)
        {
            _moviesService = moviesService;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<WatchlistMovieViewModel> movies = _dbContext.Watchlists
                .AsNoTracking()
                .Select(w => new WatchlistMovieViewModel()
                {
                    Id = w.MovieId,
                    Title = w.Movie.Title,
                    Genre = w.Movie.Genre,
                    Director = w.Movie.Director,
                    ReleaseDate = w.Movie.ReleaseDate,
                    ImageUrl = w.Movie.ImageUrl ?? DefaultImageUrl
                })
                .Take(25)
                .ToArray();

            return View(movies);
        }

        // TODO: Implement Add and Remove methods as Soft Delete operations
        [HttpPost]
        public IActionResult Add(int id)
        {
            bool movieExist = _dbContext.Movies
                .AsNoTracking()
                .Any(m => m.Id == id);

            if (!movieExist)
            {
                return NotFound();
            }

            bool isAlreadyInWatchlist = _dbContext.Watchlists
                .AsNoTracking()
                .Any(w => w.MovieId == id);

            if (!isAlreadyInWatchlist)
            {
                Watchlist watchlist = new Watchlist()
                {
                    MovieId = id
                };

                _dbContext.Watchlists.Add(watchlist);
                _dbContext.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            bool movieExist = _dbContext.Movies
                .AsNoTracking()
                .Any(m => m.Id == id);

            if (!movieExist)
            {
                return NotFound();
            }

            Watchlist? watchlistEntry = _dbContext.Watchlists
                .AsNoTracking()
                .SingleOrDefault(w => w.MovieId == id);

            if (watchlistEntry != null)
            {
                _dbContext.Watchlists.Remove(watchlistEntry);
                _dbContext.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
