using Microsoft.EntityFrameworkCore;
using NetPay.Data;
using NetPay.Data.Models;
using NetPay.DataProcessor.ImportDtos;
using NetPay.Utilities;
using System.ComponentModel.DataAnnotations;
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

        public static string ImportExpenses(NetPayContext context, string jsonString)
        {
            return string.Empty;
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
