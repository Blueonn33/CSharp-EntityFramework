namespace MoviesApp.ViewModels.Movies
{
    public class DeleteMovieViewModel
    {
        public int Id
        {
            get; set;
        }

        public string Title { get; set; } = null!;

        public string Genre { get; set; } = null!;
        public string Director { get; set; } = null!;

        public DateTime ReleaseDate
        {
            get; set;
        }

        public string? ImageUrl
        {
            get; set;
        }
    }
}
