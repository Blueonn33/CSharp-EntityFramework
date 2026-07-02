using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {

        }

        public StudentSystemContext(DbContextOptions<StudentSystemContext> options)
            : base(options) { }

        public virtual DbSet<Student> Students
        {
            get;
            set;
        } = null!;

        public virtual DbSet<Course> Courses
        {
            get;
            set;
        } = null!;

        public virtual DbSet<Homework> Homeworks
        {
            get;
            set;
        } = null!;

        public virtual DbSet<Resource> Resources
        {
            get;
            set;
        } = null!;

        public virtual DbSet<StudentCourse> StudentsCourses
        {
            get;
            set;
        } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer("Server=PREDATOR\\SQLEXPRESS;Database=StudentSystem;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity.HasKey(e => new
                {
                    e.StudentId,
                    e.CourseId
                });
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity
                    .HasMany(s => s.Homeworks)
                    .WithOne(s => s.Student)
                    .HasForeignKey(s => s.HomeworkId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity
                    .HasMany(c => c.Resources)
                    .WithOne(r => r.Course)
                    .HasForeignKey(r => r.ResourceId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasMany(c => c.Homeworks)
                    .WithOne(h => h.Course)
                    .HasForeignKey(h => h.HomeworkId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
