using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicRecordsApp.Data.Models;

public class Grade
{
    [Key]
    public int Id
    {
        get; set;
    }

    [Column(TypeName = "DECIMAL(3,2)")]
    public decimal Value
    {
        get; set;
    }

    [ForeignKey(nameof(Exam))]
    public int ExamId
    {
        get; set;
    }

    public virtual Exam Exam { get; set; } = null!;

    [ForeignKey(nameof(Student))]
    public int StudentId
    {
        get; set;
    }

    public virtual Student Student { get; set; } = null!;
}
