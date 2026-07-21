using System.ComponentModel.DataAnnotations;

namespace MoviesApp.Utilities
{
    public class ObjectValidator
    {
        public static bool IsValid(object obj, out List<string> validationErrors)
        {
            validationErrors = new List<string>();
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

            if (!isValid)
            {
                foreach (var validationResult in validationResults)
                {
                    validationErrors.Add(validationResult.ErrorMessage ?? "Unknown error");
                }
            }

            return isValid;
        }
    }
}
