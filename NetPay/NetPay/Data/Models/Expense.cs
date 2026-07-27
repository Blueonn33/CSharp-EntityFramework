using System.ComponentModel.DataAnnotations;
using static NetPay.Common.ValidationConstants;

namespace NetPay.Data.Models
{
    public class Expense
    {
        [Key]
        public int Id
        {
            get; set;
        }

        [Required]
        [MinLength(ExpenseNameMinLength)]
        [MaxLength(ExpenseNameMaxLength)]
        public string ExpenseName { get; set; } = null!;

        public decimal Amount { get; set; }

        public DateTime DueDate { get; set; }


    }
}
