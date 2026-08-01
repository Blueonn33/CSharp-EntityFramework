using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SocialNetwork.DataProcessor.ExportDTOs
{
    [XmlType("User")]
    public class ExportUsersWithFriendShipsCountAndTheirPosts
    {
        [Required]
        [StringLength(20, MinimumLength = 4)]
        [XmlElement("Username")]
        public string Username { get; set; } = null!;

        [Required]
        [XmlAttribute("Friendships")]
        public int Friendships
        {
            get; set;
        }

        [XmlArray("Posts")]
        public ExportPost[] Posts { get; set; } = Array.Empty<ExportPost>();
    }
}
