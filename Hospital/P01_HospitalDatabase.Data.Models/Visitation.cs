using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_HospitalDatabase.Data.Models
{
    public class Visitation
    {
        [Key]
        public int VisitationId
        {
            get; set;
        }

        public DateTime Date
        {
            get; set;
        }

        [MaxLength(250)] public string Comments { get; set; } = null!;

        [ForeignKey(nameof(Patient))]
        public int PatientId
        {
            get; set;
        }

        public virtual Patient Patient { get; set; } = null!;
    }
}
