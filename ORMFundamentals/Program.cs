using Microsoft.Data.SqlClient;

namespace ORMFundamentals
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Write C# Application that displays FullName + Age + Salary of all Employees from the Employees from the Research & Development department.
            string connectionString =
                @"Server=PREDATOR\SQLEXPRESS;Database=SoftUni;Trusted_Connection=True;Encrypt=False;";

            using
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            Console.WriteLine("Connection opened successfully.");
        }
    }
}
