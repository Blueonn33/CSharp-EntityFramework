using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CodeFirst.Data.Models
{
    [Comment("Represents a reply to a blog post.")]
    public class Reply
    {
        [Key]
        [Comment("Primary key for the Reply entity.")]
        public int Id
        {
            get; set;
        }

        [Required]
        [MaxLength(1000)]
        [Unicode(true)]
        [Comment("The content of the reply.")]
        public string Content { get; set; } = null!;

        [Required]
        [Comment("The Post to which this reply belongs.")]
        public int PostId
        {
            get; set;
        }
        public Post Post { get; set; } = null!;
    }
}