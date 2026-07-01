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
    }
}