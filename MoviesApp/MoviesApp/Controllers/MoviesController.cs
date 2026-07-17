using Microsoft.AspNetCore.Mvc;
using MoviesApp.Services.Interfaces;
using MoviesApp.ViewModels.Movies;

namespace MiniCinemaApp.Controllers
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

        public MoviesController(IMoviesService moviesService)
        {
            _moviesService = moviesService;
        }

        public IActionResult Index()
        {
            return View(DummyMovies);
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

            int newId = DummyMovies.Any() ? DummyMovies.Max(m => m.Id) + 1 : 1;

            DummyMovies.Add(new AllMoviesIndexViewModel
            {
                Id = newId,
                Title = model.Title,
                Genre = model.Genre,
                Director = model.Director,
                ReleaseDate = model.ReleaseDate.ToString("yyyy-MM-dd"),
                Duration = model.Duration,
                Description = model.Description,
                ImageUrl = string.IsNullOrWhiteSpace(model.ImageUrl)
                    ? DefaultImageUrl
                    : model.ImageUrl
            });

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var movie = DummyMovies.FirstOrDefault(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            if (!DateTime.TryParse(movie.ReleaseDate, out var parsedDate))
            {
                parsedDate = DateTime.MinValue;
            }

            var model = new MovieDetailsViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Genre = movie.Genre,
                Director = movie.Director,
                Duration = movie.Duration,
                ReleaseDate = parsedDate,
                Description = movie.Description,
                ImageUrl = movie.ImageUrl
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = DummyMovies.FirstOrDefault(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            if (!DateTime.TryParse(movie.ReleaseDate, out var parsedDate))
            {
                parsedDate = DateTime.Today;
            }

            var model = new EditMovieFormModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Genre = movie.Genre,
                Director = movie.Director,
                Duration = movie.Duration,
                ReleaseDate = parsedDate,
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

            var movie = DummyMovies.FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            movie.Title = model.Title;
            movie.Genre = model.Genre;
            movie.Director = model.Director;
            movie.Duration = model.Duration;
            movie.ReleaseDate = model.ReleaseDate.ToString("yyyy-MM-dd");
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
