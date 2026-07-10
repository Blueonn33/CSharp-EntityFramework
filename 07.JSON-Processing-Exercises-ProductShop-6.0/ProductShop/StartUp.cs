using ProductShop.Data;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            using ProductShopContext dbContext = new();

            string jsonFileName = "users.json";
            string jsonFilePath = GetJsonFilePath(jsonFileName);
            string jsonFileContent = File.ReadAllText(jsonFilePath);

            string result = ImportUsers(dbContext, jsonFileContent);
            Console.WriteLine(result);
        }

        // -- 02
        public static string ImportUsers(ProductShopContext dbContext, string inputJson)
        {
            return "";
        }

        private static string GetJsonFilePath(string jsonFileName)
        {
            string jsonFolderRelPath = "../../../Datasets/";
            string jsonFilePath = Path.Combine(jsonFolderRelPath, jsonFileName);

            return Path.GetFullPath(jsonFilePath);
        }
    }
}