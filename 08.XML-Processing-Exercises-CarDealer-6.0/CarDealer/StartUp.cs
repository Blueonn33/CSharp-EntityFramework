using AutoMapper.Configuration;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
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
            string xmlFileName = "suppliers.xml";
            string xmlFilePath = GetXmlFilePath(xmlFileName);
            string xmlFileContent = File.ReadAllText(xmlFilePath);

            string result = ImportSuppliers(dbContext, xmlFileContent);
            Console.WriteLine(result);
        }

        public static string ImportSuppliers(CarDealerContext dbContext, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Suppliers");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSupplierDto[]), xmlRoot);
            StringReader stringReader = new StringReader(inputXml);

            IEnumerable<ImportSupplierDto>? supplierDtos = (IEnumerable<ImportSupplierDto>?)(xmlSerializer.Deserialize(stringReader));

            if (supplierDtos == null)
            {
                supplierDtos = Array.Empty<ImportSupplierDto>();
            }

            foreach (var supplierDto in supplierDtos)
            {
                ICollection<>
            }
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