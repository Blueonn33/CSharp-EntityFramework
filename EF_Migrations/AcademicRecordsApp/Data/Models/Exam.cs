using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicRecordsApp.Data.Models;

public class Exam
{
    [Key]
    public int Id
    {
        get; set;
    }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [ForeignKey(nameof(Course))]
    public int? CourseId
    {
        get;
        set;
    }

    public virtual Course? Course
    {
        get; set;
    } = null!;

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
