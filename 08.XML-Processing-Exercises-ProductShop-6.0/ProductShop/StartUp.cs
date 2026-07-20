using ProductShop.Data;
using ProductShop.DTOs.Export;
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
            //string xmlFileName = "categories-products.xml";
            //string xmlFilePath = GetXmlFilePath(xmlFileName);
            //string xmlFileContent = File.ReadAllText(xmlFilePath);

            //string result = ImportCategoryProducts(dbContext, xmlFileContent);
            //Console.WriteLine(result);

            // Export file
            string xmlFileName = "categories-by-products.xml";
            string xmlFilePath = GetXmlResultPath(xmlFileName);

            string xmlFileContent = GetCategoriesByProductsCount(dbContext);

            File.WriteAllText(xmlFilePath, xmlFileContent);

            Console.WriteLine(xmlFileContent);
        }

        // -- 01
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

        // -- 02
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

        // -- 03
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

        // -- 04
        public static string ImportCategoryProducts(ProductShopContext dbContext, string inputXml)
        {
            IEnumerable<ImportCategoriesProductsDto>? categoriesProductsDtos = XmlSerializerWrapper
                                                                                   .Deserialize<
                                                                                       ImportCategoriesProductsDto[]>(
                                                                                       inputXml, "CategoryProducts") ??
                                                                               Array.Empty<
                                                                                   ImportCategoriesProductsDto>();

            ICollection<CategoryProduct> categoriesProductsToPersist = new List<CategoryProduct>();

            foreach (var dto in categoriesProductsDtos)
            {
                if (!IsValid(dto))
                    continue;

                if (!dbContext.Categories.Any(c => c.Id == dto.CategoryId) || !dbContext.Products.Any(p => p.Id == dto.ProductId))
                    continue;

                CategoryProduct categoryProduct = new CategoryProduct()
                {
                    CategoryId = dto.CategoryId,
                    ProductId = dto.ProductId
                };

                categoriesProductsToPersist.Add(categoryProduct);
            }

            dbContext.CategoryProducts.AddRange(categoriesProductsToPersist);
            dbContext.SaveChanges();

            return $"Successfully imported {categoriesProductsToPersist.Count}";
        }

        // -- 05
        public static string GetProductsInRange(ProductShopContext dbContext)
        {
            ExportProductsInRangeDto[] productsInRange = dbContext.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new ExportProductsInRangeDto()
                {
                    Name = p.Name,
                    Price = p.Price,
                    Buyer = p.Buyer != null ? p.Buyer!.FirstName + " " + p.Buyer.LastName : " "
                })
                .Take(10)
                .ToArray();

            string result = XmlSerializerWrapper
                .Serialize(productsInRange, "Products");

            return result;
        }

        // -- 06
        public static string GetSoldProducts(ProductShopContext dbContext)
        {
            ExportUserDto[] usersProducts = dbContext.Users
                .Where(u => u.ProductsSold.Count > 0)
                .Select(u => new ExportUserDto()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold
                        .Select(p => new ExportProductDto()
                        {
                            Name = p.Name,
                            Price = p.Price
                        })
                        .ToArray()
                })
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .ToArray();

            string result = XmlSerializerWrapper
                .Serialize(usersProducts, "Users");

            return result;
        }

        // -- 07
        public static string GetCategoriesByProductsCount(ProductShopContext dbContext)
        {
            ExportCategoryDto[] categoriesDto = dbContext.Categories
                .Select(c => new ExportCategoryDto()
                {
                    Name = c.Name,
                    Count = c.CategoryProducts
                        .Select(cp => cp.Product.Id).Count(),
                    AveragePrice = c.CategoryProducts
                        .Select(cp => cp.Product)
                        .Average(p => p.Price),
                    TotalRevenue = c.CategoryProducts
                        .Select(cp => cp.Product)
                        .Sum(p => p.Price)
                })
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.TotalRevenue)
                .ToArray();

            string result = XmlSerializerWrapper
                .Serialize(categoriesDto, "Categories");

            return result;
        }

        // -- 08

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