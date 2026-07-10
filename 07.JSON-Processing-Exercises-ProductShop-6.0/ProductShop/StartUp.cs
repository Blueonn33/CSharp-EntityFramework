using ProductShop.Models;
using System.ComponentModel.DataAnnotations;

namespace ProductShop
{
    using Data;
    using DTOs.Import;
    using Newtonsoft.Json;

    public class StartUp
    {
        public static void Main()
        {
            using ProductShopContext dbContext = new();

            //if (!dbContext.Categories.Any()
            //    && !dbContext.Products.Any()
            //    && !dbContext.Users.Any())
            //{
            //    // Reset database if no data is seeded
            //    dbContext.Database.EnsureDeleted();
            //    dbContext.Database.EnsureCreated();
            //}

            string jsonFileName = "users.json";
            string jsonFilePath = GetJsonFilePath(jsonFileName);
            string jsonFileContent = File.ReadAllText(jsonFilePath);

            string result = ImportUsers(dbContext, jsonFileContent);
            Console.WriteLine(result);
        }

        // -- 02
        public static string ImportUsers(ProductShopContext dbContext, string inputJson)
        {
            IEnumerable<ImportUserDto>? importUserDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);

            if (importUserDtos == null)
            {
                importUserDtos = Array.Empty<ImportUserDto>();
            }

            ICollection<User> usersToPersist = new List<User>();

            foreach (var userDto in importUserDtos)
            {
                if (!IsValid(userDto))
                {
                    // Skip invalid DTO records
                    continue;
                }

                // Manual mapping -> Can be omitted by using AutoMapper/Mapperly
                User newUser = new User()
                {
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Age = userDto.Age
                };

                usersToPersist.Add(newUser);
            }

            dbContext.Users.AddRange(usersToPersist);
            dbContext.SaveChanges();

            return $"Successfully imported {usersToPersist.Count}";
        }

        private static string GetJsonFilePath(string jsonFileName)
        {
            string jsonFolderRelPath = "../../../Datasets/";
            string jsonFilePath = Path.Combine(jsonFolderRelPath, jsonFileName);

            return Path.GetFullPath(jsonFilePath);
        }

        private static bool IsValid(object obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);
            ICollection<ValidationResult> validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults);

            return isValid;
        }
    }
}