using Microsoft.EntityFrameworkCore;
using NetPay.Data;
using NetPay.Data.Models;
using NetPay.Data.Models.Enums;
using NetPay.DataProcessor.ImportDtos;
using NetPay.Utilities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace NetPay.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedHousehold = "Successfully imported household. Contact person: {0}";
        private const string SuccessfullyImportedExpense = "Successfully imported expense. {0}, Amount: {1}";

        public static string ImportHouseholds(NetPayContext dbContext, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<ImportHouseholdDto>? householdDtos = XmlSerializerWrapper
                .Deserialize<ImportHouseholdDto[]>(xmlString, "Households");

            if (householdDtos == null)
            {
                return sb.ToString();
            }

            IEnumerable<Household> existingHouseholds = dbContext.Households
                .AsNoTracking()
                .ToArray();

            ICollection<Household> householdsToPersist = new List<Household>();

            foreach (var household in householdDtos)
            {
                if (!IsValid(household))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isDuplicate = existingHouseholds
                    .Any(h => h.ContactPerson.Equals(household.ContactPerson) || (household.Email != null && household.Email == h.Email) || h.PhoneNumber == household.PhoneNumber);

                // Always keep a mind on the collection with Entities to persist
                // Check for duplications there too
                isDuplicate |= householdsToPersist
                    .Any(h => h.ContactPerson == household.ContactPerson ||
                              (household.Email != null && household.Email == h.Email) ||
                              h.PhoneNumber == household.PhoneNumber);

                if (isDuplicate)
                {
                    sb.AppendLine(DuplicationDataMessage);
                    continue;
                }

                Household newHousehold = new Household()
                {
                    ContactPerson = household.ContactPerson,
                    Email = household.Email,
                    PhoneNumber = household.PhoneNumber
                };

                householdsToPersist.Add(newHousehold);

                sb.AppendLine(string.Format(SuccessfullyImportedHousehold, household.ContactPerson));
            }

            dbContext.Households.AddRange(householdsToPersist);
            dbContext.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportExpenses(NetPayContext dbContext, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<ImportExpenseDto>? expenseDtos = JsonConvert
                .DeserializeObject<ImportExpenseDto[]>(jsonString);

            if (expenseDtos == null)
            {
                return sb.ToString();
            }

            IEnumerable<int> validHouseholdIds = dbContext.Households
                .AsNoTracking()
                .Select(h => h.Id)
                .ToArray();

            IEnumerable<int> validServiceIds = dbContext.Services
                .AsNoTracking()
                .Select(s => s.Id)
                .ToArray();

            ICollection<Expense> expensesToPersist = new List<Expense>();

            foreach (var expenseDto in expenseDtos)
            {
                if (!IsValid(expenseDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isDueDateValidFormat = DateTime
                    .TryParseExact(expenseDto.DueDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                        out DateTime expenseDueDate);

                bool isPaymentStatusValid = Enum
                    .TryParse(expenseDto.PaymentStatus, out PaymentStatus expensePaymentStatus);

                if (!isDueDateValidFormat || !isPaymentStatusValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!validHouseholdIds.Contains(expenseDto.HouseholdId) ||
                    !validServiceIds.Contains(expenseDto.ServiceId))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Expense newExpense = new Expense()
                {
                    ExpenseName = expenseDto.ExpenseName,
                    Amount = expenseDto.Amount,
                    DueDate = expenseDueDate,
                    PaymentStatus = expensePaymentStatus,
                    HouseholdId = expenseDto.HouseholdId,
                    ServiceId = expenseDto.ServiceId
                };

                expensesToPersist.Add(newExpense);
                sb.AppendLine(string.Format(SuccessfullyImportedExpense, newExpense.ExpenseName, newExpense.Amount.ToString("F2")));
            }

            dbContext.Expenses.AddRange(expensesToPersist);
            dbContext.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            foreach (var result in validationResults)
            {
                string currvValidationMessage = result.ErrorMessage;
            }

            return isValid;
        }
    }
}
