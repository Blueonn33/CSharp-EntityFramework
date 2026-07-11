using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ProductShop.DTOs.Export
{
    public class ExportCategoryByProductsDto
    {
        [JsonProperty("category")]
        [Required]
        public string CategoryName { get; set; } = null!;
    }
}
