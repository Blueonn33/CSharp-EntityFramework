using System.ComponentModel.DataAnnotations;

namespace P01_HospitalDatabase.Data.Models
{
    public class Patient
    {
        [Key]
        public int PatientId
        {
            get; set;
        }

        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [MaxLength(50)]
        public string LastName { get; set; } = null!;

        [MaxLength(250)]
        public string Address { get; set; } = null!;

        [MaxLength(250)] public string Email { get; set; } = null!;

        public bool HasInsurance
        {
            get; set;
        }

        public virtual ICollection<PatientMedicament> Prescriptions
        {
            get;
            set;
        } = null!;
    }
}
