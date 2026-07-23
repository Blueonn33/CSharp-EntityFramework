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

        [MinLength(HouseholdEmailMinLength)]
        [MaxLength(HouseholdEmailMaxLength)]
        public string? Email
        {
            get; set;
        }

        [Required]
        [MaxLength(HouseholdPhoneNumberLength)]
        [RegularExpression(HouseholdPhoneNumberRegexPattern)]
        public string PhoneNumber
        {
            get;
            set;
        } = null!;
    }
}