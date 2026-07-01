using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirst.Data.Models
{
    [Comment("Represents a blog post in the system.")]
    [Index(nameof(Title))]
    //[PrimaryKey(nameof(Id), nameof(Title))] -- Composite PK
    //[PrimaryKey(nameof(Id))] -- PK
    public class Post
    {
        [Key]
        [Comment("Primary key for the Post entity.")]
        public int Id
        {
            get; set;
        }

        [Required]
        [MaxLength(200)]
        [Unicode(true)]
        [Comment("The title of the blog post.")]
        public string Title
        {
            get;
            set;
        } = null!;

        [Required]
        [MaxLength(2000)]
        [Unicode(true)]
        [Comment("The content of the blog post.")]
        public string Content { get; set; } = null!;

        [Required]
        [Comment("The Author of this post.")]
        public int AuthorId
        {
            get; set;
        }

        public DateTime CreatedOn
        {
            get; set;
        }

        public DateTime UpdatedOn
        {
            get; set;
        }

        [ForeignKey(nameof(AuthorId))]
        public Author Author
        {
            get;
            set;
        } = null!;

        public ICollection<Reply> Replies
        {
            get; set;
        } = new List<Reply>();
    }
}