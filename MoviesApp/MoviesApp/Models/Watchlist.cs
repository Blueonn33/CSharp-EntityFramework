using System.ComponentModel.DataAnnotations;

namespace MoviesApp.Models
{
    public class Watchlist
    {
        [Key]
        public int Id
        {
            get; set;
        }

        public int MovieId
        {
            get; set;
        }

        public virtual Movie Movie { get; set; } = null!;
    }
}
