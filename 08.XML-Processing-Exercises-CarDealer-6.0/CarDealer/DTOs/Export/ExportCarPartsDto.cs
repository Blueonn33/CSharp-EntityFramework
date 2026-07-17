using System.Xml.Serialization;

namespace CarDealer.DTOs.Export
{
    [XmlType("parts")]
    public class ExportCarPartsDto
    {
        [XmlElement("part")]
        public ExportPartsDto Part
        {
            get; set;
        } = null!;
    }
}
