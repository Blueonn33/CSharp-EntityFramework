using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace CarDealer.DTOs.Import
{
    [XmlType("partId")]
    public class ImportCarPartDto
    {
        [Range(0, int.MaxValue)]
        [XmlAttribute("id")]
        public int Id
        {
            get; set;
        }
    }
}