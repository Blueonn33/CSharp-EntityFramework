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
        public string? Email
        {
            get; set;
        }

        [Required]
        [XmlAttribute("phone")]
        public string PhoneNumber { get; set; } = null!;
    }
}
