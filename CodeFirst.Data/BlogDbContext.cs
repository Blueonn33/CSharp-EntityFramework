using CodeFirst.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Data
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext()
        {

        }

        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {

        }

        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Reply> Replies { get; set; } = null!;

        public DbSet<Author> Authors
        {
            get; set;
        } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                entity.ToTable("Authors");
                entity.HasKey(e => e.Id);
                entity.Property(a => a.Id)
                    .ValueGeneratedOnAdd()
                    .HasComment("Primary key for the Author entity.");
                entity.Property(a => a.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasComment("The name of the author");
            });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }
    }
}