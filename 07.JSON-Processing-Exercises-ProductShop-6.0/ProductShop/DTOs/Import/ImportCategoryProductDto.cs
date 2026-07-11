using Newtonsoft.Json;

namespace ProductShop.DTOs.Import
{
    public class ImportCategoryProductDto
    {
        [JsonRequired]
        [JsonProperty("CategoryId")]
        public int CategoryId
        {
            get; set;
        }

        [JsonRequired]
        [JsonProperty("ProductId")]
        public int ProductId
        {
            get; set;
        }
    }
}
