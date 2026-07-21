using Microsoft.AspNetCore.Mvc;
using MoviesApp.Data;
using MoviesApp.DTOs.Movie;
using MoviesApp.Mappers;
using MoviesApp.Models;
using MoviesApp.Services.Interfaces;
using MoviesApp.ViewModels.Movies;
using static MoviesApp.Common.ApplicationConstants;

namespace MoviesApp.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMoviesService _moviesService;
        private readonly MoviesAppDbContext _dbContext;
        private readonly MovieMapper _movieMapper;

        // Constructor Injection -> Instance of MoviesAppDbContext is passed from outside (ASP.NET Core)
        // To use it, store it locally in field

        public MoviesController(IMoviesService moviesService, MoviesAppDbContext dbContext, MovieMapper movieMapper)
        {
            _moviesService = moviesService;
            _dbContext = dbContext;
            _movieMapper = movieMapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Always limit the maximum count of fetched rows during DB Requests from Controller
            // Use pagination if many rows exist

            IEnumerable<GetAllMoviesDto> allMovies = await _moviesService
                .GetAllAsync();

            //IEnumerable<AllMoviesIndexViewModel> allMoviesViewModels = allMovies
            //    .Select(m => new AllMoviesIndexViewModel()
            //    {
            //        Id = m.Id,
            //        Title = m.Title,
            //        Genre = m.Genre,
            //        Director = m.Director,
            //        ReleaseDate = m.ReleaseDate.ToShortDateString(),
            //        Duration = m.Duration,
            //        ImageUrl = m.ImageUrl ?? DefaultImageUrl,
            //        Description = m.Description,
            //        IsAddedInWatchlist = m.IsAddedInWatchlist
            //    })
            //    .ToArray();

            IEnumerable<AllMoviesIndexViewModel> allMoviesViewModels = this._movieMapper
                .MapToAllMoviesIndexViewModels(allMovies);

            return View(allMoviesViewModels);
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
                ImageUrl = string.IsNullOrWhiteSpace(movie.ImageUrl)
                    ? DefaultImageUrl
                    : movie.ImageUrl
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

            _dbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var movie = _dbContext.Movies
                .Find(id);

            if (movie == null)
            {
                return NotFound();
            }

            DeleteMovieViewModel deleteViewModel = new DeleteMovieViewModel()
            {
                Id = movie.Id,
                Title = movie.Title,
                Genre = movie.Genre,
                Director = movie.Director,
                ReleaseDate = movie.ReleaseDate,
                ImageUrl = movie.ImageUrl ?? DefaultImageUrl
            };

            return View(deleteViewModel);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var movie = _dbContext.Movies
                .Find(id);

            if (movie != null)
            {
                _dbContext.Remove(movie);
                _dbContext.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}