using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            using CarDealerContext dbContext = new();

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            string jsonFileName = "suppliers.json";
            string jsonFilePath = GetJsonFilePath(jsonFileName);
            string jsonFileContent = File.ReadAllText(jsonFilePath);

            string result = ImportSuppliers(dbContext, jsonFileContent);
            Console.WriteLine(result);
        }

        // -- 09
        public static string ImportSuppliers(CarDealerContext dbContext, string inputJson)
        {
            IEnumerable<ImportSupplierDto>? importSupplierDtos =
                JsonConvert.DeserializeObject<ImportSupplierDto[]>(inputJson);

            if (importSupplierDtos == null)
            {
                importSupplierDtos = Array.Empty<ImportSupplierDto>();
            }

            ICollection<Supplier> suppliersToPersist = new List<Supplier>();

            foreach (var supplierDto in importSupplierDtos)
            {
                if (!IsValid(supplierDto))
                    continue;

                Supplier newSupplier = new Supplier()
                {
                    Name = supplierDto.Name,
                    IsImporter = supplierDto.IsImporter
                };

                suppliersToPersist.Add(newSupplier);
            }

            dbContext.Suppliers.AddRange(suppliersToPersist);
            dbContext.SaveChanges();

            return $"Successfully imported {suppliersToPersist.Count}";
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