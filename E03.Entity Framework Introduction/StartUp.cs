using SoftUni.Data;
using System.Text;

namespace SoftUni;

public class StartUp
{
    public static void Main(string[] args)
    {
        SoftUniContext dbContext = new();

        string result = GetEmployeesFullInformation(dbContext);

        Console.WriteLine(result);
    }

    public static string GetEmployeesFullInformation(SoftUniContext dbContext)
    {
        StringBuilder sb = new();

        var employees = dbContext.Employees
            .OrderBy(e => e.EmployeeId)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.MiddleName,
                e.JobTitle,
                e.Salary
            })
            .ToArray();

        foreach (var e in employees)
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:f2}");
        }

        return sb.ToString().TrimEnd();
    }
}