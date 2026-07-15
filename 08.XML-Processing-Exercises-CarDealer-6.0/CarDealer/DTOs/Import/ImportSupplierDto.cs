using System.ComponentModel.DataAnnotations;

namespace CarDealer.DTOs.Import
{
    public class ImportSupplierDto
    {
        [Required]
        public string Name
        {
            get; set;
        } = null!;

        [Required]
        public string IsImporter
        {
            get;
            set;
        } = null!;
    }
}
