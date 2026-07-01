using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models
{
    public class Team
    {
        [Key]
        public int TeamId
        {
            get; set;
        }

        [Required]
        [MaxLength(70)]
        public string Name
        {
            get;
            set;
        } = null!;

        [MaxLength(2048)]
        public string? LogoUrl
        {
            get; set;
        }

        [Required]
        [MaxLength(4)]
        public string Initials
        {
            get;
            set;
        } = null!;

        [Column(TypeName = "decimal(12, 3")]
        public decimal Budget
        {
            get; set;
        }

        public int PrimaryKitColorId
        {
            get; set;
        }

        public int SecondaryKitColorId
        {
            get; set;
        }

        public int TownId
        {
            get; set;
        }
    }
}