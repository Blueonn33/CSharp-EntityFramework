using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ProductShop.DTOs.Import
{
    public class ImportCategoryDto
    {
        [JsonProperty("name")]
        [Required]
        public string Name
        {
            get;
            set;
        } = null!;
    }
}
