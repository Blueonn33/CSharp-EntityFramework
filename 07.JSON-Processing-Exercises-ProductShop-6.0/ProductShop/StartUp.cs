using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using ProductShop.DTOs.Export;
using ProductShop.Models;
using System.ComponentModel.DataAnnotations;
using System.Text;

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

            //string jsonFileName = "categories-products.json";
            //string jsonFilePath = GetJsonFilePath(jsonFileName);
            //string jsonFileContent = File.ReadAllText(jsonFilePath);

            string jsonFileName = "users-and-products.json";
            string jsonFilePath = GetJsonResultFilePath(jsonFileName);
            //string jsonFileContent = File.ReadAllText(jsonFilePath);

            string jsonResult = GetUsersWithProducts(dbContext);

            File.WriteAllText(jsonFilePath, jsonResult, Encoding.UTF8);
            Console.WriteLine(jsonResult);
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

        // -- 04
        public static string ImportCategoryProducts(ProductShopContext dbContext, string inputJson)
        {
            IEnumerable<ImportCategoryProductDto>? categoryProductDtos =
                JsonConvert.DeserializeObject<ImportCategoryProductDto[]>(inputJson);

            if (categoryProductDtos == null)
            {
                categoryProductDtos = Array.Empty<ImportCategoryProductDto>();
            }

            ICollection<CategoryProduct> categoryProductsToPersist = new List<CategoryProduct>();

            foreach (var categoryProductDto in categoryProductDtos)
            {
                if (!IsValid(categoryProductDto))
                    continue;

                CategoryProduct categoryProduct = new CategoryProduct()
                {
                    CategoryId = categoryProductDto.CategoryId,
                    ProductId = categoryProductDto.ProductId
                };
                categoryProductsToPersist.Add(categoryProduct);
            }

            dbContext.CategoriesProducts.AddRange(categoryProductsToPersist);
            dbContext.SaveChanges();

            return $"Successfully imported {categoryProductsToPersist.Count}";
        }

        // -- 07
        public static string GetCategoriesByProductsCount(ProductShopContext dbContext)
        {
            ExportCategoryByProductsDto[] categoriesByProducts = dbContext.Categories
                .AsNoTracking()
                .Select(c => new ExportCategoryByProductsDto()
                {
                    CategoryName = c.Name,
                    ProductsCount = c.CategoriesProducts.Count,
                    AveragePrice = c.CategoriesProducts
                        .Average(cp => cp.Product.Price)
                        .ToString("F2"),
                    TotalRevenue = c.CategoriesProducts
                        .Sum(cp => cp.Product.Price)
                        .ToString("F2")
                })
                .OrderByDescending(c => c.ProductsCount)
                .ToArray();

            string jsonResult = JsonConvert
                .SerializeObject(categoriesByProducts, Formatting.Indented);

            return jsonResult;
        }

        // -- 08
        public static string GetUsersWithProducts(ProductShopContext dbContext)
        {
            var usersSoldProducts = dbContext.Users
                .AsNoTracking()
                .Where(u => u.ProductsSold.Any(p => p.BuyerId.HasValue))
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    SoldProducts = new
                    {
                        Count = u.ProductsSold.Count(p => p.BuyerId.HasValue),
                        Products = u.ProductsSold
                            .Where(p => p.BuyerId.HasValue)
                            .Select(p => new
                            {
                                p.Name,
                                p.Price
                            })
                            .ToArray()
                    }
                })
                .OrderByDescending(u => u.SoldProducts.Count)
                .ToArray();

            var usersWithSoldProducts = new
            {
                UsersCount = usersSoldProducts.Length,
                Users = usersSoldProducts
            };

            DefaultContractResolver camelCaseContractResolver = new()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            string jsonResult = JsonConvert
                .SerializeObject(usersWithSoldProducts, Formatting.Indented, new JsonSerializerSettings()
                {
                    ContractResolver = camelCaseContractResolver
                });

            return jsonResult;
        }

        private static string GetJsonFilePath(string jsonFileName)
        {
            string jsonFolderRelPath = "../../../Datasets/";
            string jsonFilePath = Path.Combine(jsonFolderRelPath, jsonFileName);

            return Path.GetFullPath(jsonFilePath);
        }

        private static string GetJsonResultFilePath(string jsonFileName)
        {
            string jsonResultFolderRelPath = "../../../Results/";
            string jsonResultFilePath = Path.Combine(jsonResultFolderRelPath, jsonFileName);

            return Path.GetFullPath(jsonResultFilePath);
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