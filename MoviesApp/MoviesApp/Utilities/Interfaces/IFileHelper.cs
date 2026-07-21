namespace MoviesApp.Utilities.Interfaces
{
    public interface IFileHelper
    {
        Task<string> ReadFileAsync(string baseDir, string fileName);
    }
}
