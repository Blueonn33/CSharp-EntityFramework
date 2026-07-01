using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models
{
    public class User
    {
        [Key]
        public int UserId
        {
            get; set;
        }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(128)]
        public string Password { get; set; } = null!;

        [Required]
        [MaxLength(254)]
        public string Email
        {
            get;
            set;
        } = null!;

        [Column(TypeName = "DECIMAL(12, 2)")]
        public decimal Balance
        {
            get; set;
        }
    }
}
