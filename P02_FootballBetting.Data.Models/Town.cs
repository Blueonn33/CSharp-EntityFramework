using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models
{
    public class Town
    {
        [Key]
        public int TownId
        {
            get; set;
        }

        [Required]
        [MaxLength(85)]
        public string Name { get; set; } = null!;

        public int CountryId
        {
            get; set;
        }

        public virtual ICollection<Team> Teams
        {
            get; set;
        } = new HashSet<Team>();
    }
}
