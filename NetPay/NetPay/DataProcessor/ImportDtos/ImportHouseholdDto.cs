using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static NetPay.Common.ValidationConstants;

namespace NetPay.DataProcessor.ImportDtos
{
    [XmlType("Household")]
    public class ImportHouseholdDto
    {
        [Required]
        [StringLength(HouseholdContactPersonMaxLength, MinimumLength = HouseholdContactPersonMinLength)]
        [XmlElement("ContactPerson")]
        public string ContactPerson { get; set; } = null!;

        [XmlElement("Email")]
        [StringLength(HouseholdEmailMaxLength, MinimumLength = HouseholdEmailMinLength)]
        public string? Email
        {
            get; set;
        }

        [Required]
        [StringLength(HouseholdPhoneNumberLength, MinimumLength = HouseholdPhoneNumberLength)]
        [RegularExpression(HouseholdPhoneNumberRegexPattern)]
        [XmlAttribute("phone")]
        public string PhoneNumber { get; set; } = null!;
    }
}
