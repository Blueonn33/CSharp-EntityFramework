using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_StudentSystem.Data.Models.Models
{
    public class Student
    {
        [Key]
        public int StudentId
        {
            get; set;
        }

        [Required]
        [MaxLength(100)]
        public string Name
        {
            get;
            set;
        } = null!;

        [StringLength(10)]
        public string PhoneNumber
        {
            get; set;
        }

        [Required]
        [Column(TypeName = "DATETIME2")]
        public DateTime RegisteredOn
        {
            get;
            set;
        }

        public DateTime Birthday
        {
            get; set;
        }
    }
}
