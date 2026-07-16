using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Import
{
    [XmlType("Customer")]
    public class ImportCustomerDto
    {
        [Required]
        [XmlElement("name")]
        public string Name { get; set; } = null!;

        [XmlElement("birthDate")]
        public DateTime BirthDate
        {
            get; set;
        }

        [XmlElement("isYoungDriver")]
        public bool IsYoungDriver
        {
            get; set;
        }
    }
}
