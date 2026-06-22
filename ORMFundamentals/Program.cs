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
            string sqlQuery = @"SELECT CONCAT(FirstName, ' ', LastName)
                                AS FullName,
                                JobTitle,
                                Salary
                                FROM Employees
                                WHERE Salary > 30000";

            using
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection);
            using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            while (sqlDataReader.Read())
            {
                string fullName = sqlDataReader.GetString(0);
                string jobTitle = sqlDataReader.GetString(1);
                decimal salary = sqlDataReader.GetDecimal(2);

                Console.WriteLine($"{fullName}, {jobTitle} - {salary:f2}");
            }

            sqlDataReader.Close();
            sqlConnection.Close();
        }
    }
}
