using Microsoft.AspNetCore.Mvc;
using MiniCinemaApp.Controllers;
using MoviesApp.Services.Interfaces;
using MoviesApp.ViewModels.Movies;

namespace MoviesApp.Controllers
{
    public class WatchlistController : Controller
    {
        private static readonly List<int> _watchlistMovieIds = new();
        private readonly IMoviesService _moviesService;

        public WatchlistController(IMoviesService moviesService)
        {
            _moviesService = moviesService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var movies = MoviesController.DummyMovies
                .Where(m => _watchlistMovieIds.Contains(m.Id))
                .ToList();

            return View(movies);
        }

        [HttpPost]
        public IActionResult Add(int id)
        {
            var exists = MoviesController.DummyMovies.Any(m => m.Id == id);
            if (!exists)
            {
                return NotFound();
            }

            if (!_watchlistMovieIds.Contains(id))
            {
                _watchlistMovieIds.Add(id);
            }

            return RedirectToAction("Index", "Movies");
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            _watchlistMovieIds.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            if (!_watchlistMovieIds.Contains(id))
            {
                return NotFound();
            }

            var movie = MoviesController.DummyMovies
                .FirstOrDefault(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            var model = new MovieDetailsViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Genre = movie.Genre,
                Director = movie.Director,
                Duration = movie.Duration,
            };

            if (DateTime.TryParse(movie.ReleaseDate, out var date))
            {
                model.ReleaseDate = date;
            }
            else
            {
                model.ReleaseDate = DateTime.Today;
            }

            model.Description = movie.Description;
            model.ImageUrl = movie.ImageUrl;

            return View(model);
        }
    }
}
