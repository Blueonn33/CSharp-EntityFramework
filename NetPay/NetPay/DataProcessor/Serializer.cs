using Microsoft.EntityFrameworkCore;
using NetPay.Data;
using NetPay.Data.Models.Enums;
using NetPay.DataProcessor.ExportDtos;
using NetPay.Utilities;

namespace NetPay.DataProcessor
{
    public class Serializer
    {
        public static string ExportHouseholdsWhichHaveExpensesToPay(NetPayContext dbContext)
        {
            ExportHouseholdUnpaidExpensesDto[] householdUnpaidExpensesDtos = dbContext.Households
                .AsNoTracking()
                .Where(h => h.Expenses.Any(e => e.PaymentStatus != PaymentStatus.Paid))
                .Select(h => new ExportHouseholdUnpaidExpensesDto()
                {
                    ContactPerson = h.ContactPerson,
                    Email = h.Email,
                    PhoneNumber = h.PhoneNumber,
                    UnpaidExpenses = h.Expenses
                        .Where(e => e.PaymentStatus != PaymentStatus.Paid)
                        .Select(e => new ExportUnpaidExpensesDto()
                        {
                            ExpenseName = e.ExpenseName,
                            Amount = e.Amount.ToString("f2"),
                            DueDate = e.DueDate.ToString("yyyy-MM-dd"),
                            ServiceName = e.Service.ServiceName
                        })
                        .OrderBy(e => e.DueDate)
                        .ThenBy(e => e.Amount)
                        .ToArray()
                })
                .OrderBy(h => h.ContactPerson)
                .ToArray();

            string xmlResult = XmlSerializerWrapper
                .Serialize(householdUnpaidExpensesDtos, "Households");

            return xmlResult;
        }

        public static string ExportAllServicesWithSuppliers(NetPayContext context)
        {
            throw new NotImplementedException();
        }
    }
}
