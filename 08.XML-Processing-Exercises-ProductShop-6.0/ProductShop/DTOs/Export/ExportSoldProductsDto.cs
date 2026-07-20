using ProductShop.DTOs.Export;
using System.Xml.Serialization;

public class ExportSoldProductsDto
{
    [XmlElement("count")]
    public int Count
    {
        get; set;
    }

    [XmlArray("products")]
    [XmlArrayItem("Product")]
    public ExportProductDto[] Products
    {
        get; set;
    }
}