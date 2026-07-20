using System.Xml.Serialization;

namespace ProductShop.DTOs.Export
{
    [XmlType("SoldProducts")]
    public class ExportSoldProductsDto
    {
        [XmlElement("count")]
        public int Count
        {
            get; set;
        }

        [XmlArray("products")]
        [XmlElement("Product")]
        public ExportProductDto[] Products
        {
            get;
            set;
        } = null!;
    }
}
