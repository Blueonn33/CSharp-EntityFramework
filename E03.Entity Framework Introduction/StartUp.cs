using SoftUni.Data;
using SoftUni.Models;
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

    public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext dbContext)
    {
        const string rndDepartmentName = "Research and Development";
        StringBuilder sb = new();

        var rndEmployees = dbContext.Employees
            .OrderBy(e => e.EmployeeId)
            .Where(e => e.Department.Name == rndDepartmentName)
            .Select(e => new
            {
                e.FirstName,
                e.LastName,
                DepartmentName = e.Department.Name,
                e.Salary
            })
            .OrderBy(e => e.Salary)
            .ThenByDescending(e => e.FirstName)
            .ToArray();

        foreach (var e in rndEmployees)
        {
            sb.AppendLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - ${e.Salary:f2}");
        }

        return sb.ToString().TrimEnd();
    }

    public static string AddNewAddressToEmployee(SoftUniContext dbContext)
    {
        //StringBuilder sb = new();

        // 1.
        //Address newAddress = new Address()
        //{
        //    AddressText = "Vitoshka 15",
        //    TownId = 4
        //};

        //dbContext.Addresses.Add(newAddress);

        Employee nakovEmployee = dbContext.Employees
            .First(e => e.LastName == "Nakov");

        // 2.
        nakovEmployee.Address = new Address()
        {
            AddressText = "Vitoshka 15",
            TownId = 4
        };

        dbContext.SaveChanges();

        IEnumerable<string> employeesAddresses = dbContext.Employees
            .Where(e => e.AddressId != null)
            .OrderByDescending(e => e.AddressId)
            .Select(e => e.Address!.AddressText)
            .Take(10)
            .ToArray();

        return string.Join(Environment.NewLine, employeesAddresses);
    }

    public static string DeleteProjectById(SoftUniContext dbContext)
    {
        Project? projectIdToDelete = dbContext.Projects
            .Find(2);

        if (projectIdToDelete != null)
        {
            IQueryable<EmployeeProject> employeeProjectsToDelete = dbContext.EmployeesProjects
                .Where(ep => ep.ProjectId == 2);

            dbContext.EmployeesProjects.RemoveRange(employeeProjectsToDelete);

            dbContext.Projects.Remove(projectIdToDelete);
            dbContext.SaveChanges();
        }

        IEnumerable<string> top10ProjectNames = dbContext.Projects
            .Select(p => p.Name)
            .Take(10)
            .ToArray();

        return string.Join(Environment.NewLine, top10ProjectNames);
    }
}