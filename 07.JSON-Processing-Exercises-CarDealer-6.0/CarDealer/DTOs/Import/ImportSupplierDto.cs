using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CarDealer.DTOs.Import
{
    public class ImportSupplierDto
    {
        [JsonProperty("name")]
        [Required]
        public string Name
        {
            get; set;
        } = null!;

        [JsonProperty("isImporter")]
        public bool IsImporter
        {
            get; set;
        }
    }
}
