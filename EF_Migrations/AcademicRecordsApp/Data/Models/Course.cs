using System.ComponentModel.DataAnnotations;

namespace AcademicRecordsApp.Data.Models
{
    public class Course
    {
        [Key]
        public int Id
        {
            get; set;
        }

        [Required]
        [MaxLength(100)]
        public string Name
        {
            get; set;
        } = null!;

        public virtual ICollection<Student> Students { get; set; } = new List<Student>();

        public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
    }
}