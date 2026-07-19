using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using ProductShop.Utilities;
using System.ComponentModel.DataAnnotations;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            using ProductShopContext dbContext = new ProductShopContext();
            dbContext.Database.EnsureCreated();

            // Import file
            string xmlFileName = "users.xml";
            string xmlFilePath = GetXmlFilePath(xmlFileName);
            string xmlFileContent = File.ReadAllText(xmlFilePath);

            string result = ImportUsers(dbContext, xmlFileContent);
            Console.WriteLine(result);
        }

        public static string ImportUsers(ProductShopContext dbContext, string inputXml)
        {
            IEnumerable<ImportUsersDto>? usersDtos = XmlSerializerWrapper
                .Deserialize<ImportUsersDto[]>(inputXml, "Users");

            if (usersDtos == null)
            {
                usersDtos = Array.Empty<ImportUsersDto>();
            }

            ICollection<User> usersToPersist = new List<User>();

            foreach (var userDto in usersDtos)
            {
                if (!IsValid(userDto))
                {
                    continue;
                }

                User newUser = new User()
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Age = userDto.Age
                };

                usersToPersist.Add(newUser);
            }

            dbContext.AddRange(usersToPersist);
            dbContext.SaveChanges();

            return $"Successfully imported {usersToPersist.Count}";
        }

        private static string GetXmlFilePath(string fileName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string xmlDirectory = Path.Combine(currentDirectory, "../../../Datasets", fileName);

            return Path.GetFullPath(xmlDirectory);
        }

        private static string GetXmlResultPath(string fileName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string xmlDirectory = Path.Combine(currentDirectory, "../../../Results", fileName);

            return Path.GetFullPath(xmlDirectory);
        }

        private static bool IsValid(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            ICollection<ValidationResult> validationResults = new List<ValidationResult>();

            bool isValid = Validator
                .TryValidateObject(obj, validationContext, validationResults);

            return isValid;
        }
    }
}