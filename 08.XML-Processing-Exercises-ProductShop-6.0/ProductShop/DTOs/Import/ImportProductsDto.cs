using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Import
{
    [XmlType("Product")]
    public class ImportProductsDto
    {
        [Required]
        [XmlElement("name")]
        public string Name { get; set; } = null!;

        [XmlElement("price")]
        public decimal Price
        {
            get; set;
        }

        [Range(0, int.MaxValue)]
        [XmlElement("sellerId")]
        public int SellerId
        {
            get; set;
        }

        [Range(0, int.MaxValue)]
        [XmlElement("buyerId")]
        public int BuyerId
        {
            get; set;
        }
    }
}
