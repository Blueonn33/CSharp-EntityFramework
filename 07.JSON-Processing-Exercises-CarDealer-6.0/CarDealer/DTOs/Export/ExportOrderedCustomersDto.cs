using Newtonsoft.Json;

namespace CarDealer.DTOs.Export
{
    public class ExportOrderedCustomersDto
    {
        [JsonProperty("Name")]
        public string Name
        {
            get;
            set;
        } = null!;

        [JsonProperty("BirthDate")]
        public DateTime BirthDate
        {
            get;
            set;
        }

        [JsonProperty("IsYoungDriver")]
        public bool IsYoungDriver
        {
            get; set;
        }
    }
}
