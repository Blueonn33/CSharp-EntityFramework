using MoviesApp.DTOs.Json;
using MoviesApp.Services.Interfaces;
using MoviesApp.Utilities.Interfaces;
using Newtonsoft.Json;
using System.Text.Json;

namespace MoviesApp.Services
{
    public class ImportService : IImportService
    {
        private readonly IFileHelper _fileHelper;
        private readonly IConfiguration _appConfiguration;

        public ImportService(IFileHelper fileHelper, IConfiguration appConfiguration)
        {
            _fileHelper = fileHelper;
            _appConfiguration = appConfiguration;
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

            foreach (var movieDto in movieDtos)
            {
                
            }
        }

        public async Task<int> ImportFromXmlAsync(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
