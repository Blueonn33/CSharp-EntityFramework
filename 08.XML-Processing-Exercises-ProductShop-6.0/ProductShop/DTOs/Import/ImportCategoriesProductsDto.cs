using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ProductShop.DTOs.Import
{
    [XmlType("CategoryProduct")]
    public class ImportCategoriesProductsDto
    {
        [Required]
        [XmlElement("CategoryId")]
        public int CategoryId
        {
            get; set;
        }

        [Required]
        [XmlElement("ProductId")]
        public int ProductId
        {
            get; set;
        }
    }
}
