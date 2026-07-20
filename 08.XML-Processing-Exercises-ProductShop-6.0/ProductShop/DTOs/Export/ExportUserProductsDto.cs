using ProductShop.DTOs.Export;
using System.Xml.Serialization;

public class ExportUserProductsDto
{
    [XmlElement("firstName")]
    public string FirstName
    {
        get; set;
    }

    [XmlElement("lastName")]
    public string LastName
    {
        get; set;
    }

    [XmlElement("age")]
    public int? Age
    {
        get; set;
    }

    [XmlElement("SoldProducts")]
    public ExportSoldProductsDto SoldProducts
    {
        get; set;
    }
}