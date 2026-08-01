using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SocialNetwork.DataProcessor.ExportDTOs
{
    [XmlType("Post")]
    public class ExportPost
    {
        [Required]
        [StringLength(300, MinimumLength = 5)]
        [XmlElement("Content")]
        public string Content { get; set; } = null!;

        [Required]
        [XmlElement("CreatedAt")]
        public string CreatedAt { get; set; } = null!;
    }
}
