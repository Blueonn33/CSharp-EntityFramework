using CarDealer.Data;
using CarDealer.DTOs.Export;
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
            //string xmlFileName = "sales.xml";
            //string xmlFilePath = GetXmlFilePath(xmlFileName);
            //string xmlFileContent = File.ReadAllText(xmlFilePath);

            //string result = ImportSales(dbContext, xmlFileContent);
            //Console.WriteLine(result);

            // Export file
            string xmlFileName = "cars-and-parts.xml";
            string xmlFilePath = GetXmlResultPath(xmlFileName);

            string xmlFileContent = GetCarsWithTheirListOfParts(dbContext);

            File.WriteAllText(xmlFilePath, xmlFileContent);

            Console.WriteLine(xmlFileContent);
        }

        // 09
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

        // 10
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

        // 11
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

        // -- 12
        public static string ImportCustomers(CarDealerContext dbContext, string inputXml)
        {
            IEnumerable<ImportCustomerDto>? customerDtos =
                XmlSerializerWrapper.Deserialize<ImportCustomerDto[]>(inputXml, "Customers");

            if (customerDtos == null)
            {
                customerDtos = Array.Empty<ImportCustomerDto>();
            }

            ICollection<Customer> customersToPersist = new List<Customer>();

            foreach (var customerDto in customerDtos)
            {
                if (!IsValid(customerDto))
                {
                    continue;
                }

                Customer newCustomer = new Customer()
                {
                    Name = customerDto.Name,
                    BirthDate = customerDto.BirthDate,
                    IsYoungDriver = customerDto.IsYoungDriver
                };

                customersToPersist.Add(newCustomer);
            }

            dbContext.Customers.AddRange(customersToPersist);
            dbContext.SaveChanges();

            return $"Successfully imported {customersToPersist.Count}";
        }

        // -- 13
        public static string ImportSales(CarDealerContext dbContext, string inputXml)
        {
            IEnumerable<ImportSaleDto>? saleDtos = XmlSerializerWrapper.Deserialize<ImportSaleDto[]>(inputXml, "Sales");

            if (saleDtos == null)
            {
                saleDtos = Array.Empty<ImportSaleDto>();
            }

            IEnumerable<int> existingCarIds = dbContext.Cars
                .AsNoTracking()
                .Select(c => c.Id)
                .ToArray();

            ICollection<Sale> salesToPersist = new List<Sale>();

            foreach (var saleDto in saleDtos)
            {
                if (!IsValid(saleDto))
                {
                    continue;
                }

                if (!existingCarIds.Contains(saleDto.CarId))
                {
                    continue;
                }

                Sale newSale = new Sale()
                {
                    CarId = saleDto.CarId,
                    CustomerId = saleDto.CustomerId,
                    Discount = saleDto.Discount,
                };

                salesToPersist.Add(newSale);
            }

            dbContext.Sales.AddRange(salesToPersist);
            dbContext.SaveChanges();

            return $"Successfully imported {salesToPersist.Count}";
        }

        // -- 14
        public static string GetCarsWithDistance(CarDealerContext dbContext)
        {
            ExportCarsDistanceDto[] carsWithDistance = dbContext.Cars
                .AsNoTracking()
                .Select(c => new ExportCarsDistanceDto()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance
                })
                .Where(c => c.TraveledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ToArray();

            string result = XmlSerializerWrapper
                .Serialize(carsWithDistance, "cars");

            return result;
        }

        // -- 15
        public static string GetCarsFromMakeBmw(CarDealerContext dbContext)
        {
            ExportBMWDto[] bmw = dbContext.Cars
                .AsNoTracking()
                .Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .Select(c => new ExportBMWDto()
                {
                    Id = c.Id,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance
                })
                .ToArray();

            string result = XmlSerializerWrapper
                .Serialize(bmw, "cars");

            return result;
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
                    PartsCount = s.Parts.Count,
                })
                .ToArray();

            string result = XmlSerializerWrapper
                .Serialize(localSuppliers, "suppliers");

            return result;
        }

        // -- 17
        public static string GetCarsWithTheirListOfParts(CarDealerContext dbContext)
        {
            ExportCarsDto[] cars = dbContext.Cars
                .AsNoTracking()
                .OrderByDescending(c => c.PartsCars.Sum(pc => pc.Part.Price))
                .ThenByDescending(c => c.TraveledDistance)
                .ThenBy(c => c.Model)
                .Select(c => new ExportCarsDto()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance,
                    Parts = c.PartsCars
                        .Select(pc => new ExportPartsDto()
                        {
                            Name = pc.Part.Name,
                            Price = pc.Part.Price
                        })
                })
                .ToArray();

            string result = XmlSerializerWrapper
                .Serialize(cars, "cars");

            return result;
        }

        // --  19
        public static string GetSalesWithAppliedDiscount(CarDealerContext dbContext)
        {
            ExportSaleDiscountsDto[] salesWithDiscounts = dbContext.Sales
                .AsNoTracking()
                .Select(s => new ExportSaleDiscountsDto()
                {
                    Car = new ExportSaleDiscountsCarDto()
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TraveledDistance.ToString()
                    },
                    Discount = ((int)(s.Discount)).ToString(),
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartsCars.Sum(pc => pc.Part.Price),
                    DiscountedPrice = (s.Car.PartsCars
                            .Sum(pc => pc.Part.Price)) * (1 - s.Discount / 100.0m)
                })
                .ToArray();

            string result = XmlSerializerWrapper
                .Serialize(salesWithDiscounts, "sales");

            return result;
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