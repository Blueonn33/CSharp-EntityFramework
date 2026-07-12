using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            using CarDealerContext dbContext = new();

            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            // Import
            //string jsonFileName = "sales.json";
            //string jsonFilePath = GetJsonFilePath(jsonFileName);
            //string inputJson = File.ReadAllText(jsonFilePath);
            //string result = ImportSales(dbContext, inputJson);
            //Console.WriteLine(result);

            // Export
            string jsonFileName = "sales-discounts.json";
            string jsonFilePath = GetJsonResultFilePath(jsonFileName);

            string result = GetSalesWithAppliedDiscount(dbContext);
            File.WriteAllText(jsonFilePath, result, Encoding.UTF8);
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
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .ToList()
                .Select(c => new ExportOrderedCustomersDto()
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy"),
                    IsYoungDriver = c.IsYoungDriver
                })
                .ToArray();

            string jsonResult = JsonConvert.SerializeObject(orderedCustomers, Formatting.Indented);

            return jsonResult;
        }

        // -- 15
        public static string GetCarsFromMakeToyota(CarDealerContext dbContext)
        {
            ExportCarsFromMakeToyotaDto[] toyotaCars = dbContext.Cars
                .AsNoTracking()
                .Where(c => c.Make == "Toyota")
                .Select(c => new ExportCarsFromMakeToyotaDto()
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .ToArray();

            string jsonResult = JsonConvert.SerializeObject(toyotaCars, Formatting.Indented);
            return jsonResult;
        }

        // -- 16
        public static string GetLocalSuppliers(CarDealerContext dbContext)
        {
            ExportLocalSuppliersDto[] localSuppliers = dbContext.Suppliers
                .AsNoTracking()
                .Where(s => s.IsImporter == false)
                .Select(s => new ExportLocalSuppliersDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToArray();

            string jsonResult = JsonConvert.SerializeObject(localSuppliers, Formatting.Indented);
            return jsonResult;
        }

        // -- 17
        public static string GetCarsWithTheirListOfParts(CarDealerContext dbContext)
        {
            var carsWithParts = dbContext.Cars
                .AsNoTracking()
                .Select(c => new
                {
                    car = new
                    {
                        Make = c.Make,
                        Model = c.Model,
                        TraveledDistance = c.TraveledDistance
                    },
                    parts = c.PartsCars
                        .Select(pc => new
                        {
                            Name = pc.Part.Name,
                            Price = pc.Part.Price.ToString("F2")
                        })
                        .ToArray()
                })
                .ToArray();

            string jsonResult = JsonConvert.SerializeObject(carsWithParts, Formatting.Indented);

            return jsonResult;
        }

        // -- 18
        public static string GetTotalSalesByCustomer(CarDealerContext dbContext)
        {
            ExportTotalSalesByCustomerDto[] totalSalesByCustomer = dbContext.Customers
                .AsNoTracking()
                .Where(c => c.Sales.Any())
                .Select(c => new ExportTotalSalesByCustomerDto
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales
                        .SelectMany(s => s.Car.PartsCars)
                        .Sum(pc => pc.Part.Price)
                })
                .OrderByDescending(c => c.SpentMoney)
                .ThenByDescending(c => c.BoughtCars)
                .ToArray();

            string jsonResult = JsonConvert.SerializeObject(totalSalesByCustomer, Formatting.Indented);
            return jsonResult;
        }

        // -- 19
        public static string GetSalesWithAppliedDiscount(CarDealerContext dbContext)
        {
            var sales = dbContext.Sales
                .AsNoTracking()
                .Select(s => new
                {
                    car = new
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TraveledDistance
                    },
                    customerName = s.Customer.Name,
                    discount = s.Discount,
                    price = s.Car.PartsCars.Sum(pc => pc.Part.Price),
                    priceWithDiscount = s.Car.PartsCars.Sum(pc => pc.Part.Price) * (1 - s.Discount / 100m)
                })
                .Take(10)
                .ToArray();

            var salesWithFormattedValues = sales
                .Select(s => new
                {
                    s.car,
                    s.customerName,
                    discount = s.discount.ToString("F2", CultureInfo.InvariantCulture),
                    price = s.price.ToString("F2", CultureInfo.InvariantCulture),
                    priceWithDiscount = s.priceWithDiscount.ToString("F2", CultureInfo.InvariantCulture)
                })
                .ToArray();

            return JsonConvert.SerializeObject(salesWithFormattedValues, Formatting.Indented);
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