namespace MusicHub.Data.Models
{
    public class Album
    {
        public int Id
        {
            get; set;
        }

        public string Name { get; set; } = null!;

        public DateOnly ReleaseDate
        {
            get; set;
        }

        public decimal Price
        {
            get; set;
        }

        public int? ProducerId
        {
            get; set;
        }

        public virtual Producer? Producer { get; set; }

        public virtual ICollection<Song> Songs
        {
            get;
            set;
        } = new HashSet<Song>();
    }
}
