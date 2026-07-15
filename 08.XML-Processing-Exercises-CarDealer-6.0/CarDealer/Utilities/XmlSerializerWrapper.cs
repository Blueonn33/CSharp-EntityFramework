using System.Xml.Serialization;

namespace CarDealer.Utilities
{
    public static class XmlSerializerWrapper
    {
        public static T? Deserialize<T>(string inputXml, string rootName)
            where T : class, new()
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRoot);
            StringReader stringReader = new StringReader(inputXml);

            T? result = (T?)xmlSerializer.Deserialize(stringReader);

            return result;
        }
    }
}
