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

            string jsonFileName = "categories.json";
            string jsonFilePath = GetJsonFilePath(jsonFileName);
            string jsonFileContent = File.ReadAllText(jsonFilePath);

            string result = ImportCategories(dbContext, jsonFileContent);
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

            ICollection<Product> productsToPersist = new List<Product>();

            foreach (var productDto in productDtos)
            {
                if (!IsValid(productDto))
                {
                    continue;
                }

                bool isSellerIdValid = validUserIds.Contains(productDto.SellerId);
                bool isBuyerIdValid = (!productDto.BuyerId.HasValue) || validUserIds.Contains(productDto.BuyerId.Value);

                //if (!isSellerIdValid || !isBuyerIdValid)
                //{
                //    continue;
                //}

                Product newProduct = new Product()
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    SellerId = productDto.SellerId,
                    BuyerId = productDto.BuyerId,
                };

                productsToPersist.Add(newProduct);
            }

            dbContext.Products.AddRange(productsToPersist);
            dbContext.SaveChanges();

            return $"Successfully imported {productsToPersist.Count}";
        }

        // -- 03
        public static string ImportCategories(ProductShopContext dbContext, string inputJson)
        {
            IEnumerable<ImportCategoryDto>? categoryDtos = JsonConvert
                .DeserializeObject<ImportCategoryDto[]>(inputJson);

            if (categoryDtos == null)
            {
                categoryDtos = Array.Empty<ImportCategoryDto>();
            }

            ICollection<Category> categoriesToPersist = new List<Category>();

            foreach (var categoryDto in categoryDtos)
            {
                if (!IsValid(categoryDto))
                {
                    continue;
                }

                Category newCategory = new Category()
                {
                    Name = categoryDto.Name
                };
                categoriesToPersist.Add(newCategory);
            }

            dbContext.Categories.AddRange(categoriesToPersist);
            dbContext.SaveChanges();

            return $"Successfully imported {categoriesToPersist.Count}";
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