using CarDealer.Data;

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
            string xmlFileContext = File.ReadAllText(xmlFilePath);

        }

        public static string ImportSuppliers(CarDealerContext dbContext, string inputXml)
        {
            return null;
        }

        private static string GetXmlFilePath(string fileName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string xmlDirectory = Path.Combine(currentDirectory, "../../../Datasets", fileName);

            return Path.GetFullPath(xmlDirectory);
        }
    }
}