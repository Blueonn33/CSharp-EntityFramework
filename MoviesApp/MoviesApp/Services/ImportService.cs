using MoviesApp.Services.Interfaces;

namespace MoviesApp.Services
{
    public class ImportService : IImportService
    {
        private readonly IConfiguration _appConfiguration;

        public ImportService(IConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        public async Task<int> ImportFromJsonAsync(string filePath)
        {
            string? datasetsPath = _appConfiguration.GetValue<string>("Seeding:DatasetPath");
        }

        public async Task<int> ImportFromXmlAsync(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
