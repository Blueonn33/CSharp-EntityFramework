using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Import
{
    [XmlType("Part")]
    public class ImportPartDto
    {
        [Required]
        [XmlElement("name")]
        public string Name
        {
            get;
            set;
        } = null!;

        [Required]
        [XmlElement("price")]
        public string Price
        {
            get;
            set;
        } = null!;

        [Range(0, int.MaxValue)]
        [XmlElement("quantity")]
        public int Quantity
        {
            get; set;
        }

        [Range(0, int.MaxValue)]
        [XmlElement("supplierId")]
        public int SupplierId
        {
            get; set;
        }
    }
}