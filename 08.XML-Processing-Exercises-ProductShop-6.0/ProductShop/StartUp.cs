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
            //dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            // Import file
            string xmlFileName = "categories.xml";
            string xmlFilePath = GetXmlFilePath(xmlFileName);
            string xmlFileContent = File.ReadAllText(xmlFilePath);

            string result = ImportCategories(dbContext, xmlFileContent);
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

            dbContext.Users.AddRange(usersToPersist);
            dbContext.SaveChanges();

            return $"Successfully imported {usersToPersist.Count}";
        }

        public static string ImportProducts(ProductShopContext dbContext, string inputXml)
        {
            IEnumerable<ImportProductsDto>? productsDtos = XmlSerializerWrapper
                .Deserialize<ImportProductsDto[]>(inputXml, "Products");

            if (productsDtos == null)
            {
                productsDtos = Array.Empty<ImportProductsDto>();
            }

            ICollection<Product> productsToPersist = new List<Product>();

            foreach (var productDto in productsDtos)
            {
                if (!IsValid(productDto))
                    continue;

                Product newProduct = new Product()
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    SellerId = productDto.SellerId,
                    BuyerId = productDto.BuyerId
                };

                productsToPersist.Add(newProduct);
            }

            dbContext.Products.AddRange(productsToPersist);
            dbContext.SaveChanges();

            return $"Successfully imported {productsToPersist.Count}";
        }

        public static string ImportCategories(ProductShopContext dbContext, string inputXml)
        {
            IEnumerable<ImportCategoriesDto>? categoriesDtos = XmlSerializerWrapper
                .Deserialize<ImportCategoriesDto[]>(inputXml, "Categories") ?? Array.Empty<ImportCategoriesDto>();

            ICollection<Category> categoriesToPersist = new List<Category>();

            foreach (var dto in categoriesDtos)
            {
                if (!IsValid(dto))
                    continue;

                Category category = new Category()
                {
                    Name = dto.Name
                };

                categoriesToPersist.Add(category);
            }

            dbContext.Categories.AddRange(categoriesToPersist);
            dbContext.SaveChanges();

            return $"Successfully imported {categoriesToPersist.Count}";
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