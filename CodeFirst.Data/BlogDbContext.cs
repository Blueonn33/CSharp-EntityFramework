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
            // Fluent API
            //modelBuilder.Entity<Author>(entity =>
            //{
            //    entity.ToTable("Authors");
            //    entity.HasKey(e => e.Id);
            //    entity.Property(a => a.Id)
            //        .ValueGeneratedOnAdd()
            //        .HasComment("Primary key for the Author entity.");
            //    entity.Property(a => a.Name)
            //        .IsRequired()
            //        .IsUnicode(true)
            //        .HasMaxLength(100)
            //        .HasComment("The name of the author");
            //});

            modelBuilder.Entity<Reply>()
                .HasOne(r => r.Post)
                .WithMany(p => p.Replies)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Restrict);

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