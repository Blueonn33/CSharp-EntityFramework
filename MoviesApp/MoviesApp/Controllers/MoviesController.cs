using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Models;
using MoviesApp.Services.Interfaces;
using MoviesApp.ViewModels.Movies;

namespace MoviesApp.Controllers
{
    public class MoviesController : Controller
    {
        private const string DefaultImageUrl =
            "https://img.freepik.com/free-vector/cinema-film-production-realistic-transparent-composition-with-isolated-image-clapper-with-empty-fields-vector-illustration_1284-66163.jpg?semt=ais_incoming&w=740&q=80";

        public static readonly List<AllMoviesIndexViewModel> DummyMovies = new()
        {
            new AllMoviesIndexViewModel
            {
                Id = 1,
                Title = "Sample Movie",
                Genre = "Action",
                Director = "John Doe",
                ReleaseDate = "2024-01-01",
                Duration = 120,
                Description = "Dummy action movie used for the exercise.",
                ImageUrl = DefaultImageUrl
            },
            new AllMoviesIndexViewModel
            {
                Id = 2,
                Title = "Second Movie",
                Genre = "Comedy",
                Director = "Jane Smith",
                ReleaseDate = "2023-06-15",
                Duration = 95,
                Description = "Dummy comedy movie used for the exercise.",
                ImageUrl = DefaultImageUrl
            }
        };

        private readonly IMoviesService _moviesService;
        private readonly MoviesAppDbContext _dbContext;

        // Constructor Injection -> Instance of MoviesAppDbContext is passed from outside (ASP.NET Core)
        // To use it, store it locally in field

        public MoviesController(IMoviesService moviesService, MoviesAppDbContext dbContext)
        {
            _moviesService = moviesService;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Always limit the maximum count of fetched rows during DB Requests from Controller
            // Use pagination if many rows exist

            IEnumerable<AllMoviesIndexViewModel> allMovies = _dbContext.Movies
                .AsNoTracking()
                .OrderBy(m => m.Title)
                .ThenByDescending(m => m.ReleaseDate)
                .Select(m => new AllMoviesIndexViewModel()
                {
                    Id = m.Id,
                    Title = m.Title,
                    Genre = m.Genre,
                    Director = m.Director,
                    ReleaseDate = m.ReleaseDate.ToShortDateString(),
                    Description = m.Description,
                    Duration = m.Duration,
                    ImageUrl = m.ImageUrl ?? DefaultImageUrl
                })
                .Take(25)
                .ToArray();

            return View(allMovies);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new AddMovieFormModel
            {
                ReleaseDate = DateTime.Today
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(AddMovieFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Movie newMovie = new Movie()
            {
                Title = model.Title,
                Genre = model.Genre,
                Director = model.Director,
                Duration = model.Duration,
                Description = model.Description,
                ImageUrl = model.ImageUrl
            };

            _dbContext.Movies.Add(newMovie);
            _dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            Movie? movie = _dbContext.Movies
                .Find(id);

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
                ReleaseDate = movie.ReleaseDate,
                Description = movie.Description,
                ImageUrl = movie.ImageUrl
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Movie? movie = _dbContext.Movies
                .Find(id);

            if (movie == null)
            {
                return NotFound();
            }

            var model = new EditMovieFormModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Genre = movie.Genre,
                Director = movie.Director,
                Duration = movie.Duration,
                ReleaseDate = movie.ReleaseDate,
                Description = movie.Description,
                ImageUrl = movie.ImageUrl
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id, EditMovieFormModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Movie? movie = _dbContext.Movies
                .Find(id);

            if (movie == null)
            {
                return NotFound();
            }

            movie.Title = model.Title;
            movie.Genre = model.Genre;
            movie.Director = model.Director;
            movie.Duration = model.Duration;
            movie.ReleaseDate = model.ReleaseDate;
            movie.Description = model.Description;
            movie.ImageUrl = string.IsNullOrWhiteSpace(model.ImageUrl)
                ? DefaultImageUrl
                : model.ImageUrl;

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            var movie = DummyMovies.FirstOrDefault(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var movie = DummyMovies.FirstOrDefault(m => m.Id == id);

            if (movie != null)
            {
                DummyMovies.Remove(movie);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
