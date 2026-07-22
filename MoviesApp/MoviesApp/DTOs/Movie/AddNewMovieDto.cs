namespace MoviesApp.DTOs.Movie
{
    public class AddNewMovieDto
    {
        public string Title { get; set; } = null!;

        public string Genre { get; set; } = null!;

        public string Director { get; set; } = null!;

        public DateOnly ReleaseDate
        {
            get; set;
        }

        public int Duration
        {
            get; set;
        }

        public string Description { get; set; } = null!;

        public string? ImageUrl
        {
            get; set;
        }
    }
}
