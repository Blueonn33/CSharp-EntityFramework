using Newtonsoft.Json;

namespace CarDealer.DTOs.Export
{
    public class ExportLocalSuppliersDto
    {
        [JsonProperty("Id")]
        public int Id
        {
            get; set;
        }

        [JsonProperty("Name")]
        public string Name
        {
            get; set;
        } = null!;

        [JsonProperty("PartsCount")]
        public int PartsCount
        {
            get; set;
        }
    }
}
