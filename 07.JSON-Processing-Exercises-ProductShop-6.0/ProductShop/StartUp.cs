using Microsoft.EntityFrameworkCore;
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

            string jsonFileName = "products.json";
            string jsonFilePath = GetJsonFilePath(jsonFileName);
            string jsonFileContent = File.ReadAllText(jsonFilePath);

            string result = ImportProducts(dbContext, jsonFileContent);
            Console.WriteLine(result);
        }

        // -- 01
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

        // -- 02
        public static string ImportProducts(ProductShopContext dbContext, string inputJson)
        {
            IEnumerable<ImportProductDto>? productDtos = JsonConvert
                .DeserializeObject<ImportProductDto[]>(inputJson);

            if (productDtos == null)
            {
                productDtos = Array.Empty<ImportProductDto>();
            }

            IEnumerable<int> validUserIds = dbContext.Users
                .AsNoTracking()
                .Select(u => u.Id)
                .ToArray();

            foreach (var productDto in productDtos)
            {
                if (!IsValid(productDto))
                {
                    continue;
                }

                Product newProduct = new Product();
                {

                }
            }
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