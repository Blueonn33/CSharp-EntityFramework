using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CarDealer.DTOs.Import
{
    public class ImportPartDto
    {
        [JsonProperty("name")]
        [Required]
        public string Name
        {
            get; set;
        } = null!;

        [JsonProperty("price")]
        [Required]
        public decimal Price
        {
            get; set;
        }

        [JsonProperty("quantity")]
        [Required]
        public int Quantity
        {
            get; set;
        }

        [JsonProperty("supplierId")]
        [Required]
        public int SupplierId
        {
            get; set;
        }
    }
}
