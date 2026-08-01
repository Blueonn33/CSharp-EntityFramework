using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Data.Models
{
    public class User
    {
        [Key]
        public int Id
        {
            get; set;
        }

        [Required]
        [MinLength(4)]
        [MaxLength(20)]
        public string Username { get; set; } = null!;

        [Required]
        [MinLength(8)]
        [MaxLength(60)]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;

        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();

        public virtual ICollection<UserConversation> UsersConversations
        {
            get; set;
        } = new HashSet<UserConversation>();
    }
}
