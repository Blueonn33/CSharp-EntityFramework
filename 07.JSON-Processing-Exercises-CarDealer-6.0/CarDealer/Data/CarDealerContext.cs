using CarDealer.Models;
using Microsoft.EntityFrameworkCore;

namespace CarDealer.Data
{
    public class CarDealerContext : DbContext
    {
        public CarDealerContext()
        {
        }

        public CarDealerContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Car> Cars
        {
            get; set;
        }
        public DbSet<Customer> Customers
        {
            get; set;
        }
        public DbSet<Part> Parts
        {
            get; set;
        }
        public DbSet<PartCar> PartsCars
        {
            get; set;
        }
        public DbSet<Sale> Sales
        {
            get; set;
        }
        public DbSet<Supplier> Suppliers
        {
            get; set;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PartCar>(e =>
            {
                e.HasKey(k => new { k.CarId, k.PartId });
            });

            // A Customer can buy many Cars.

            modelBuilder.Entity<Customer>(e =>
            {
                e.HasKey(c => c.Id);

                e.HasMany(c => c.Sales)
                    .WithOne(s => s.Customer)
                    .HasForeignKey(s => s.CustomerId);
            });

            // One Supplier can supply many Parts and each Part can be delivered by only one Supplier

            modelBuilder.Entity<Supplier>(e =>
            {
                e.HasKey(s => s.Id);

                e.HasMany(s => s.Parts)
                    .WithOne(p => p.Supplier)
                    .HasForeignKey(p => p.SupplierId);
            });

            modelBuilder.Entity<Sale>(e =>
            {
                e.HasKey(s => s.Id);

                e.HasOne(s => s.Car)
                    .WithMany(c => c.Sales)
                    .HasForeignKey(c => c.CarId);
            });
        }
    }
}
