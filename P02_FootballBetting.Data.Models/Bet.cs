using P02_FootballBetting.Data.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models
{
    public class Bet
    {
        public int BetId
        {
            get; set;
        }

        [Column(TypeName = "DECIMAL(8, 2)")]
        public decimal Amount
        {
            get; set;
        }

        public Prediction Prediction
        {
            get; set;
        }

        [Column(TypeName = "DATETIME2")]
        public DateTime DateTime
        {
            get; set;
        }

        [ForeignKey(nameof(User))]
        public int UserId
        {
            get; set;
        }

        public virtual User User { get; set; } = null!;

        [ForeignKey(nameof(Game))]
        public int GameId
        {
            get; set;
        }

        public virtual Game Game { get; set; } = null!;
    }
}