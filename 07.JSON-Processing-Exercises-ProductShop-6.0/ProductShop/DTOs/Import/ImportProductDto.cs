using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ProductShop.DTOs.Import
{
    public class ImportProductDto
    {
        // Required won't throw an exception, it will return false from IsValid() method in case of error
        [Required]
        [JsonProperty("Name")]
        public string Name
        {
            get; set;
        } = null!;

        [JsonRequired]
        [JsonProperty("Price")]
        public decimal Price
        {
            get; set;
        }

        [JsonRequired]
        [JsonProperty("SellerId")]
        public int SellerId
        {
            get; set;
        }

        [JsonProperty("BuyerId")]
        public int? BuyerId
        {
            get; set;
        }
    }
}