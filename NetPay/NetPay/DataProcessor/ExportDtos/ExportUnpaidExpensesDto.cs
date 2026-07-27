using System.Xml.Serialization;

namespace NetPay.DataProcessor.ExportDtos
{
    [XmlType("Expense")]
    public class ExportUnpaidExpensesDto
    {
        [XmlElement("ExpenseName")]
        public string ExpenseName { get; set; } = null!;

        public decimal Amount
        {
            get; set;
        }
    }
}
