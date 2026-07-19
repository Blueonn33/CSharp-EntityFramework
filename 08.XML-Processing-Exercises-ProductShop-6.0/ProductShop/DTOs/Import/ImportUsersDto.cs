using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Import
{
    [XmlType("User")]
    public class ImportUsersDto
    {
        [Required]
        [XmlElement("firstName")]
        public string FirstName { get; set; } = null!;

        [Required]
        [XmlElement("lastName")]
        public string LastName { get; set; } = null!;

        [XmlElement("age")]
        public int Age
        {
            get; set;
        }
    }
}
