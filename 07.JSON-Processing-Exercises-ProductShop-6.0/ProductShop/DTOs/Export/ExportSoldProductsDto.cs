using Newtonsoft.Json;

namespace ProductShop.DTOs.Export
{
    public class ExportSoldProductsDto
    {
        [JsonProperty("firstName")]
        public string FirstName
        {
            get; set;
        }
    }
}
