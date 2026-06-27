using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CodeFirst.Data.Models
{
    [Comment("Represents an author of blog posts.")]
    public class Author
    {
        [Key]
        [Comment("Primary key for the Author entity.")]
        public int Id
        {
            get; set;
        }

        [Required]
        [MaxLength(100)]
        [Unicode(true)]
        [Comment("The name of the author.")]
        public string Name
        {
            get; set;
        }

        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}