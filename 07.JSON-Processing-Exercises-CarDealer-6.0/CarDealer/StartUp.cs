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

            string jsonFileName = "customers.json";
            string jsonFilePath = GetJsonFilePath(jsonFileName);
            string jsonFileContent = File.ReadAllText(jsonFilePath);

            string result = ImportCustomers(dbContext, jsonFileContent);
            Console.WriteLine(result);
        }

        // -- 09
        public static string ImportSuppliers(CarDealerContext dbContext, string inputJson)
        {
            IEnumerable<ImportSupplierDto>? importSupplierDtos = JsonConvert.DeserializeObject<ImportSupplierDto[]>(inputJson);

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

            return $"Successfully imported {suppliersToPersist.Count}.";
        }

        // -- 10
        public static string ImportParts(CarDealerContext dbContext, string inputJson)
        {
            IEnumerable<ImportPartDto>? importPartDtos = JsonConvert.DeserializeObject<ImportPartDto[]>(inputJson);

            if (importPartDtos == null)
            {
                importPartDtos = Array.Empty<ImportPartDto>();
            }

            ICollection<Part> partsToPersist = new List<Part>();

            foreach (var partDto in importPartDtos)
            {
                if (!IsValid(partDto) || !dbContext.Suppliers.Any(s => s.Id == partDto.SupplierId))
                    continue;

                Part newPart = new Part()
                {
                    Name = partDto.Name,
                    Price = partDto.Price,
                    Quantity = partDto.Quantity,
                    SupplierId = partDto.SupplierId,
                };

                partsToPersist.Add(newPart);
            }

            dbContext.Parts.AddRange(partsToPersist);
            dbContext.SaveChanges();

            return $"Successfully imported {partsToPersist.Count}.";
        }

        // -- 11
        public static string ImportCars(CarDealerContext dbContext, string inputJson)
        {
            IEnumerable<ImportCarDto>? importCarDtos = JsonConvert.DeserializeObject<ImportCarDto[]>(inputJson);

            if (importCarDtos == null)
            {
                importCarDtos = Array.Empty<ImportCarDto>();
            }

            ICollection<Car> carsToPersist = new List<Car>();

            foreach (var carDto in importCarDtos)
            {
                if (carDto == null)
                    continue;

                Car newCar = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TraveledDistance = carDto.TraveledDistance,
                };

                foreach (var partId in carDto.PartsId.Distinct())
                {
                    if (dbContext.Parts.Any(p => p.Id == partId))
                    {
                        newCar.PartsCars.Add(new PartCar
                        {
                            PartId = partId
                        });
                    }
                }

                carsToPersist.Add(newCar);
            }

            dbContext.Cars.AddRange(carsToPersist);
            dbContext.SaveChanges();

            return $"Successfully imported {carsToPersist.Count}.";
        }

        // -- 12
        public static string ImportCustomers(CarDealerContext dbContext, string inputJson)
        {

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