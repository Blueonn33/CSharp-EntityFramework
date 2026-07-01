using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models
{
    public class Color
    {
        [Key]
        public int ColorId
        {
            get; set;
        }

        [Required]
        [MaxLength(40)]
        public string Name
        {
            get;
            set;
        } = null!;
    }
}
