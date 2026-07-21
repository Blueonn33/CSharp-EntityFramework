using MoviesApp.Utilities.Interfaces;

namespace MoviesApp.Utilities
{
    public class FileHelper : IFileHelper
    {
        public async Task<string> ReadFileAsync(string baseDir, string fileName)
        {
            if (string.IsNullOrWhiteSpace(baseDir) || !Directory.Exists(baseDir))
            {
                throw new InvalidOperationException("Base directory path is not configured");
            }

            string fullPath = Path.Combine(baseDir, fileName);

            if (!File.Exists(fullPath))
            {
                throw new InvalidOperationException($"File {fullPath} not found!");
            }

            string jsonFileContent = await File.ReadAllTextAsync(fullPath);

            return jsonFileContent;
        }
    }
}
