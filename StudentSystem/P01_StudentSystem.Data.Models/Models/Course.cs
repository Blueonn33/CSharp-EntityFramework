using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_StudentSystem.Data.Models.Models
{
    public class Course
    {
        [Key]
        public int CourseId
        {
            get; set;
        }

        [Required]
        [MaxLength(80)]
        public string Name
        {
            get; set;
        }
            = null!;

        public string? Description
        {
            get; set;
        }

        [Column(TypeName = "DATETIME2")]
        public DateTime StartDate
        {
            get; set;
        }

        [Column(TypeName = "DATETIME2")]
        public DateTime EndDate
        {
            get; set;
        }

        [Required]
        [Column(TypeName = "DECIMAL(6, 2)")]
        public decimal Price
        {
            get; set;
        }

        public virtual ICollection<StudentCourse> StudentsCourses
        {
            get; set;
        }
            = new HashSet<StudentCourse>();

        public virtual ICollection<Resource> Resources
        {
            get; set;
        }
            = new HashSet<Resource>();

        public virtual ICollection<Homework> Homeworks
        {
            get; set;
        }
            = new HashSet<Homework>();
    }
}
