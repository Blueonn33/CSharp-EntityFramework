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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
