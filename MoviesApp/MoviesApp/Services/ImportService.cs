using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MoviesApp.Data;
using MoviesApp.DTOs.Json;
using MoviesApp.Models;
using MoviesApp.Services.Interfaces;
using MoviesApp.Utilities.Interfaces;
using Newtonsoft.Json;
using System.Globalization;
using static MoviesApp.Utilities.ObjectValidator;

namespace MoviesApp.Services
{
    public class ImportService : IImportService
    {
        private readonly IFileHelper _fileHelper;
        private readonly IConfiguration _appConfiguration;
        private readonly ILogger<ImportService> _logger;
        private readonly MoviesAppDbContext _dbContext;

        public ImportService(IFileHelper fileHelper, IConfiguration appConfiguration, ILogger<ImportService> logger, MoviesAppDbContext dbContext)
        {
            _fileHelper = fileHelper;
            _appConfiguration = appConfiguration;
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<int> ImportFromJsonAsync(string fileName)
        {
            string? datasetsPath = _appConfiguration.GetValue<string>("Seeding:DatasetPath");

            string jsonFileContent = await _fileHelper
                .ReadFileAsync(datasetsPath, fileName);

            IEnumerable<ImportJsonMovieDto>? movieDtos = JsonConvert
                .DeserializeObject<ImportJsonMovieDto[]>(jsonFileContent);

            if (movieDtos == null)
            {
                return 0;
            }

            bool moviesAlreadyExist = _dbContext.Movies.Any();

            if (moviesAlreadyExist)
            {
                _logger.LogInformation("Movies already exist in the database! Import from JSON will be skipped!");
                return 0;
            }

            ICollection<Movie> moviesToImport = new List<Movie>();

            foreach (var movieDto in movieDtos)
            {
                if (!IsValid(movieDto, out List<string> validationErrors))
                {
                    _logger.LogInformation("Movie was not imported during import movies from JSON! Please, see error message below");

                    foreach (var error in validationErrors)
                    {
                        _logger.LogError(error);
                    }

                    continue;
                }

                bool isReleaseDateValidFormat = DateTime.TryParseExact(movieDto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime releaseDate);

                if (!isReleaseDateValidFormat)
                {
                    _logger.LogError($"Movie DTO {movieDto.Id} Release Date is in incorrect format");
                    continue;
                }

                Movie movie = new Movie()
                {
                    Id = movieDto.Id,
                    Title = movieDto.Title,
                    Genre = movieDto.Genre,
                    ReleaseDate = releaseDate,
                    Director = movieDto.Director,
                    Duration = movieDto.Duration,
                    Description = movieDto.Description,
                    ImageUrl = movieDto.ImageUrl
                };

                moviesToImport.Add(movie);
            }

            await SeedMoviesAsync(moviesToImport);

            return moviesToImport.Count;
        }

        public async Task<int> ImportFromXmlAsync(string fileName)
        {
            string? datasetsPath = _appConfiguration.GetValue<string>("Seeding:DatasetPath");

            string xmlFileContent = await _fileHelper
                .ReadFileAsync(datasetsPath, fileName);
        }

        private async Task SeedMoviesAsync(IEnumerable<Movie> moviesToImport)
        {
            IDbContextTransaction transaction = await _dbContext.Database
                .BeginTransactionAsync();

            try
            {
                await _dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Movies ON");

                _dbContext.Movies.AddRange(moviesToImport);
                await _dbContext.SaveChangesAsync();

                await _dbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Movies OFF");

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                await transaction.RollbackAsync();
            }
        }
    }
}
