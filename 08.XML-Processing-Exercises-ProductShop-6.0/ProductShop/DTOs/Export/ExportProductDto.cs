using System.Xml.Serialization;

public class ExportProductDto
{
    [XmlElement("name")]
    public string Name
    {
        get; set;
    }

    [XmlElement("price")]
    public decimal Price
    {
        get; set;
    }
}