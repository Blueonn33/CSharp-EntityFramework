using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models
{
    public class Player
    {
        [Key]
        public int PlayerId
        {
            get; set;
        }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = null!;

        public byte SquadNumber
        {
            get; set;
        }

        public bool IsInjured { get; set; } = false;

        public int PositionId
        {
            get; set;
        }

        [ForeignKey(nameof(Team))]
        public int? TeamId
        {
            get; set;
        }

        public virtual Team? Team
        {
            get; set;
        }

        public int TownId
        {
            get; set;
        }
    }
}
