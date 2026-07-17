namespace MiniCinemaApp.Data.Models
{
    public class Watchlist
    {
        public int Id { get; set; }

        public int MovieId { get; set; }

        public Movie Movie { get; set; } = null!;
    }
}
