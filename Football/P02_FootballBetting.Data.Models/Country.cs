using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models
{
    public class Country
    {
        [Key]
        public int CountryId
        {
            get; set;
        }

        [Required]
        [MaxLength(60)]
        public string Name
        {
            get;
            set;
        } = null!;

        public virtual ICollection<Town> Towns
        {
            get; set;
        }
            = new HashSet<Town>();
    }
}
