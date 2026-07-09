using AcademicRecordsApp.Data;

namespace AcademicRecordsApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            AcademicRecordsDbContext dbContext = new();

            Console.WriteLine("Database connected...");
        }
    }
}