using System.ComponentModel.DataAnnotations;
using static NetPay.Common.ValidationConstants;

namespace NetPay.DataProcessor.ImportDtos
{
    public class ImportExpenseDto
    {
        [Required]
        [StringLength(ExpenseNameMaxLength, MinimumLength = ExpenseNameMinLength)]
        public string ExpenseName { get; set; } = null!;

        [Range(typeof(decimal), ExpenseAmountRangeMinValue, ExpenseAmountRangeMaxValue)]
        public decimal Amount
        {
            get; set;
        }

        [Required]
        public string DueDate
        {
            get;
            set;
        } = null!;

        [Required]
        public string PaymentStatus { get; set; } = null!;

        public int HouseholdId
        {
            get; set;
        }

        public int ServiceId
        {
            get; set;
        }
    }
}