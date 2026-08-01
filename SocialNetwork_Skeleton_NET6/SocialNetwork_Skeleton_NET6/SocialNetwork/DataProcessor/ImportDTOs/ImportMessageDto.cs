using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SocialNetwork.DataProcessor.ImportDTOs
{
    [XmlType("Message")]
    public class ImportMessageDto
    {
        [Required]
        [StringLength(200, MinimumLength = 1)]
        [XmlElement("Content")]
        public string Content { get; set; } = null!;

        [Required]
        [XmlElement("Status")]
        public string Status
        {
            get; set;
        } = null!;

        [XmlElement("ConversationId")]
        public int ConversationId
        {
            get; set;
        }

        [XmlElement("SenderId")]
        public int SenderId
        {
            get; set;
        }

        [Required]
        [XmlAttribute("SentAt")]
        public string SentAt
        {
            get;
            set;
        } = null!;
    }
}
