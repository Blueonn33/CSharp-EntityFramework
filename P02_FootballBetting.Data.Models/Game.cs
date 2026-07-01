using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models
{
    public class Game
    {
        [Key]
        public int GameId
        {
            get; set;
        }

        [ForeignKey(nameof(HomeTeam))]
        public int HomeTeamId
        {
            get; set;
        }

        public virtual Team HomeTeam
        {
            get;
            set;
        } = null!;

        [ForeignKey(nameof(AwayTeam))]
        public int AwayTeamId
        {
            get; set;
        }

        public virtual Team AwayTeam
        {
            get;
            set;
        } = null!;

        public byte HomeTeamGoals
        {
            get; set;
        }
        public byte AwayTeamGoals
        {
            get; set;
        }

        [Column(TypeName = "DECIMAL(6, 2)")]
        public decimal HomeTeamBetRate
        {
            get; set;
        }

        [Column(TypeName = "DECIMAL(6, 2)")]
        public decimal AwayTeamBetRate
        {
            get; set;
        }

        [Column(TypeName = "DECIMAL(6, 2)")]
        public decimal DrawBetRate
        {
            get; set;
        }

        [Column(TypeName = "SMALLDATETIME")]
        public DateTime DateTime
        {
            get; set;
        }

        [MaxLength(7)]
        public string? Result
        {
            get; set;
        }
    }
}
