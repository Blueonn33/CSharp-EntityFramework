using System.Xml.Serialization;

namespace CarDealer.DTOs.Export
{
    [XmlType("car")]
    public class ExportBMWDto
    {
        [XmlAttribute("id")]
        public int Id
        {
            get;
            set;
        }

        //[XmlAttribute("make")]
        //[XmlIgnore]
        //public string Make
        //{
        //    get; set;
        //} = null!;

        [XmlAttribute("model")]
        public string Model
        {
            get; set;
        } = null!;

        [XmlAttribute("traveled-distance")]
        public long TraveledDistance
        {
            get;
            set;
        }
    }
}
