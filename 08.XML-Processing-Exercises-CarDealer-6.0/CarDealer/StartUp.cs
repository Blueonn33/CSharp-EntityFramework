using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using CarDealer.Utilities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            using CarDealerContext dbContext = new CarDealerContext();
            dbContext.Database.EnsureCreated();

            // Read file
            string xmlFileName = "cars.xml";
            string xmlFilePath = GetXmlFilePath(xmlFileName);
            string xmlFileContent = File.ReadAllText(xmlFilePath);

            string result = ImportCars(dbContext, xmlFileContent);
            Console.WriteLine(result);
        }

        public static string ImportSuppliers(CarDealerContext dbContext, string inputXml)
        {
            IEnumerable<ImportSupplierDto>? supplierDtos =
                XmlSerializerWrapper.Deserialize<ImportSupplierDto[]>(inputXml, "Suppliers");

            if (supplierDtos == null)
            {
                supplierDtos = Array.Empty<ImportSupplierDto>();
            }

            ICollection<Supplier> suppliersToPersist = new List<Supplier>();

            foreach (var supplierDto in supplierDtos)
            {
                if (!IsValid(supplierDto))
                {
                    continue;
                }

                bool isImporterPropValid = bool
                    .TryParse(supplierDto.IsImporter, out bool isImporterValid);

                if (!isImporterPropValid)
                {
                    continue;
                }

                Supplier newSupplier = new Supplier()
                {
                    Name = supplierDto.Name,
                    IsImporter = isImporterValid
                };

                suppliersToPersist.Add(newSupplier);
            }

            dbContext.Suppliers.AddRange(suppliersToPersist);
            dbContext.SaveChanges();

            return $"Successfully imported {suppliersToPersist.Count}";
        }

        public static string ImportParts(CarDealerContext dbContext, string inputXml)
        {
            IEnumerable<ImportPartDto>? partDtos = XmlSerializerWrapper
                .Deserialize<ImportPartDto[]>(inputXml, "Parts");

            if (partDtos == null)
            {
                partDtos = Array.Empty<ImportPartDto>();
            }

            IEnumerable<int> existingSupplierIds = dbContext
                .Suppliers
                .AsNoTracking()
                .Select(s => s.Id)
                .ToArray();

            ICollection<Part> partsToPersist = new List<Part>();

            foreach (var partDto in partDtos)
            {
                if (!IsValid(partDto))
                {
                    continue;
                }

                bool isPricePropValid = decimal
                    .TryParse(partDto.Price, out decimal priceVal);

                if (!isPricePropValid)
                {
                    continue;
                }

                if (!existingSupplierIds.Contains(partDto.SupplierId))
                {
                    continue;
                }

                Part newPart = new Part()
                {
                    Name = partDto.Name,
                    Price = priceVal,
                    Quantity = partDto.Quantity,
                    SupplierId = partDto.SupplierId
                };

                partsToPersist.Add(newPart);
            }

            dbContext.Parts.AddRange(partsToPersist);
            dbContext.SaveChanges();

            return $"Successfully imported {partsToPersist.Count}";
        }

        public static string ImportCars(CarDealerContext dbContext, string inputXml)
        {
            IEnumerable<ImportCarDto>? carDtos = XmlSerializerWrapper
                .Deserialize<ImportCarDto[]>(inputXml
                    , "Cars");

            if (carDtos == null)
            {
                carDtos = Array.Empty<ImportCarDto>();
            }

            IEnumerable<int> existingPartIds = dbContext.Parts
                .AsNoTracking()
                .Select(p => p.Id)
                .ToArray();

            ICollection<Car> carsToPersist = new List<Car>();

            foreach (var carDto in carDtos)
            {
                if (!IsValid(carDto))
                {
                    continue;
                }

                Car newCar = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TraveledDistance = carDto.TraveledDistance
                };

                IEnumerable<int> uniquePartIds = carDto.Parts
                    .Select(p => p.Id)
                    .Distinct()
                    .ToArray();

                ICollection<PartCar> carParts = new List<PartCar>();

                foreach (var partId in uniquePartIds)
                {
                    if (!existingPartIds.Contains(partId))
                    {
                        continue;
                    }

                    PartCar newPartCar = new PartCar()
                    {
                        PartId = partId,
                        Car = newCar
                    };

                    carParts.Add(newPartCar);
                }

                newCar.PartsCars = carParts;
                carsToPersist.Add(newCar);
            }

            dbContext.Cars.AddRange(carsToPersist);
            dbContext.SaveChanges();

            return $"Successfully imported {carsToPersist.Count}";
        }

        private static string GetXmlFilePath(string fileName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string xmlDirectory = Path.Combine(currentDirectory, "../../../Datasets", fileName);

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