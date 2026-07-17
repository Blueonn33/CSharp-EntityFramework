using MoviesApp.Services.Interfaces;

namespace MoviesApp.Services
{
    public class ImportService : IImportService
    {
        public async Task<int> ImportFromJsonAsync(string filePath)
        {
            throw new NotImplementedException();
        }

        public async Task<int> ImportFromXmlAsync(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
