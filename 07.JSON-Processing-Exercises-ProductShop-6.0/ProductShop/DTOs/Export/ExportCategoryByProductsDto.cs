using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ProductShop.DTOs.Export
{
    public class ExportCategoryByProductsDto
    {
        [JsonProperty("category")]
        [Required]
        public string CategoryName
        {
            get; set;
        } = null!;

        [JsonProperty("productsCount")]
        public int ProductsCount
        {
            get; set;
        }

        [JsonProperty("averagePrice")]
        public string AveragePrice
        {
            get; set;
        } = null!;

        [JsonProperty("totalRevenue")]
        public string TotalRevenue
        {
            get; set;
        } = null!;
    }
}
