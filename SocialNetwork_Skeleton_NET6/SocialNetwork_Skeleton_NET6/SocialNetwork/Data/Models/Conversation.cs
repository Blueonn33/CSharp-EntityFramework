using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Data.Models
{
    public class Conversation
    {
        [Key]
        public int Id
        {
            get; set;
        }

        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        public string Title { get; set; } = null!;

        [Required]
        public DateTime StartedAt
        {
            get; set;
        }

        public virtual ICollection<UserConversation> UsersConversations
        {
            get;
            set;
        } = new HashSet<UserConversation>();

        public virtual ICollection<Message> Messages { get; set; } = new HashSet<Message>();
    }
}
