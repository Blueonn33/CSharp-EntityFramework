using EntityFrameworkIntro.Models;

namespace SoftUni.Models
{
    public class Project
    {
        public int ProjectId
        {
            get; set;
        }
        public string Name { get; set; } = null!;
        public string? Description
        {
            get; set;
        }
        public DateTime StartDate
        {
            get; set;
        }
        public DateTime? EndDate
        {
            get; set;
        }

        //public virtual ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();

        public virtual ICollection<EmployeeProject> EmployeesProjects
        {
            get;
            set;
        } = new HashSet<EmployeeProject>();
    }
}
