using System.Xml.Serialization;

namespace NetPay.DataProcessor.ExportDtos
{
    [XmlType("Expense")]
    public class ExportUnpaidExpensesDto
    {
        [XmlElement("ExpenseName")]
        public string ExpenseName { get; set; } = null!;

        [XmlElement("Amount")]
        public string Amount
        {
            get;
            set;
        } = null!;

        [XmlElement("PaymentDate")]
        public string DueDate { get; set; } = null!;

        [XmlElement("ServiceName")]
        public string ServiceName { get; set; } = null!;
    }
}