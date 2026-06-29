namespace EntityFrameworkIntro.Models
{
    public class Address
    {
        public int AddressId
        {
            get; set;
        }
        public string AddressText { get; set; } = null!;
        public int? TownId
        {
            get; set;
        }

        public virtual Town? Town
        {
            get; set;
        }

        /* Inline initialization is preferred for virtual navigation collections */
        public virtual ICollection<Employee> Employees
        {
            get;
            set;
        } = new HashSet<Employee>();
    }
}
