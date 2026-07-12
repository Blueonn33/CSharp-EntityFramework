using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            using CarDealerContext dbContext = new();

            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            //string jsonFileName = "ordered-customers.json";
            string jsonFileName = "sales.json";
            string jsonFilePath = GetJsonFilePath(jsonFileName);
            string inputJson = File.ReadAllText(jsonFilePath);
            //string jsonFilePath = GetJsonResultFilePath(jsonFileName);

            //string result = GetOrderedCustomers(dbContext);
            //File.WriteAllText(jsonFilePath, result, Encoding.UTF8);
            //Console.WriteLine(result);

            string result = ImportSales(dbContext, inputJson);
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
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carDtos = JsonConvert.DeserializeObject<List<ImportCarDto>>(inputJson)
                          ?? new List<ImportCarDto>();

            var validPartIds = context.Parts
                .Select(p => p.Id)
                .ToHashSet();

            var cars = new List<Car>();

            foreach (var dto in carDtos)
            {
                var car = new Car
                {
                    Make = dto.Make,
                    Model = dto.Model,
                    TraveledDistance = dto.TraveledDistance
                };

                foreach (var partId in dto.PartsId.Distinct())
                {
                    if (validPartIds.Contains(partId))
                    {
                        car.PartsCars.Add(new PartCar
                        {
                            PartId = partId
                        });
                    }
                }

                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}.";
        }

        // -- 12
        public static string ImportCustomers(CarDealerContext dbContext, string inputJson)
        {
            IEnumerable<ImportCustomerDto>? importCustomerDtos = JsonConvert.DeserializeObject<ImportCustomerDto[]>(inputJson);

            if (importCustomerDtos == null)
            {
                importCustomerDtos = Array.Empty<ImportCustomerDto>();
            }

            ICollection<Customer> customersToPersist = new List<Customer>();

            foreach (var customerDto in importCustomerDtos)
            {
                if (!IsValid(customerDto))
                    continue;

                Customer newCustomer = new Customer
                {
                    Name = customerDto.Name,
                    BirthDate = customerDto.BirthDate,
                    IsYoungDriver = customerDto.IsYoungDriver,
                };

                customersToPersist.Add(newCustomer);
            }

            dbContext.Customers.AddRange(customersToPersist);
            dbContext.SaveChanges();

            return $"Successfully imported {customersToPersist.Count}.";
        }

        // -- 13
        public static string ImportSales(CarDealerContext dbContext, string inputJson)
        {
            var saleDtos = JsonConvert.DeserializeObject<List<ImportSaleDto>>(inputJson);

            var sales = new List<Sale>();

            foreach (var dto in saleDtos)
            {
                //if (!dbContext.Cars.Any(c => c.Id == dto.CarId))
                //    continue;

                var sale = new Sale
                {
                    CarId = dto.CarId,
                    CustomerId = dto.CustomerId,
                    Discount = dto.Discount
                };

                sales.Add(sale);
            }

            dbContext.Sales.AddRange(sales);
            dbContext.SaveChanges();

            return $"Successfully imported {sales.Count}.";
        }

        // -- 14
        public static string GetOrderedCustomers(CarDealerContext dbContext)
        {
            ExportOrderedCustomersDto[] orderedCustomers = dbContext.Customers
                .AsNoTracking()
                .Select(c => new ExportOrderedCustomersDto()
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate,
                    IsYoungDriver = c.IsYoungDriver
                })
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver == false)
                .ToArray();

            string jsonResult = JsonConvert.SerializeObject(orderedCustomers, Formatting.Indented);

            Console.WriteLine(dbContext.Suppliers.Count());

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