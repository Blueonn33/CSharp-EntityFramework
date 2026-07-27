using System.ComponentModel.DataAnnotations;
using static NetPay.Common.ValidationConstants;

namespace NetPay.Data.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(SupplierNameMinLength)]
        [MaxLength(SupplierNameMaxLength)]
        public string SupplierName { get; set; } = null!;
    }
}
