using System.ComponentModel.DataAnnotations;
using static NetPay.Common.ValidationConstants;

namespace NetPay.Data.Models
{
    public class Service
    {
        [Key]
        public int Id
        {
            get; set;
        }

        [Required]
        [MinLength(ServiceNameMinLength)]
        [MaxLength(ServiceNameMaxLength)]
        public string ServiceName { get; set; } = null!;

        public virtual ICollection<SupplierService> SuppliersServices { get; set; } = new HashSet<SupplierService>();

        public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
