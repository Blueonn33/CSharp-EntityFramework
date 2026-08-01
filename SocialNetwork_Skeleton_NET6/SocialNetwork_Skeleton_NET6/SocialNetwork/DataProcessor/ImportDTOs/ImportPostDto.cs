using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.DataProcessor.ImportDTOs
{
    public class ImportPostDto
    {
        [Required]
        [StringLength(300, MinimumLength = 5)]
        public string Content { get; set; } = null!;

        [Required]
        public string CreatedAt { get; set; } = null!;

        public int CreatorId
        {
            get; set;
        }
    }
}
