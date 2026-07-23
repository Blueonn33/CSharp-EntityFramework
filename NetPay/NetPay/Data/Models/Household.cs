using System.ComponentModel.DataAnnotations;
using static NetPay.Common.ValidationConstants;

namespace NetPay.Data.Models
{
    public class Household
    {
        [Key]
        public int Id
        {
            get; set;
        }

        [Required]
        [MinLength(HouseholdContactPersonMinLength)]
        [MaxLength(HouseholdContactPersonMaxLength)]
        public string ContactPerson { get; set; } = null!;

        public string? Email
        {
            get; set;
        }
    }
}