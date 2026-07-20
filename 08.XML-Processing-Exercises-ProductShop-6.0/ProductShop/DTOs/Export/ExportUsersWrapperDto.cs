using System.Xml.Serialization;
using ProductShop.DTOs.Export;

[XmlRoot("Users")]
public class ExportUsersWrapperDto
{
    [XmlElement("count")]
    public int Count
    {
        get; set;
    }

    [XmlArray("users")]
    [XmlArrayItem("User")]
    public ExportUserProductsDto[] Users
    {
        get; set;
    }
}