using AcademicRecordsApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AcademicRecordsApp.Data;

public partial class AcademicRecordsDbContext : DbContext
{
    public AcademicRecordsDbContext()
    {
    }

    public AcademicRecordsDbContext(DbContextOptions<AcademicRecordsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Exam> Exams
    {
        get;
        set;
    } = null!;

    public virtual DbSet<Grade> Grades
    {
        get;
        set;
    } = null!;

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseSqlServer(Configuration.ConnectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exams__3214EC07ADE67E82");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Grades__3214EC07BE7E02CB");

            entity.Property(e => e.Value).HasColumnType("decimal(3, 2)");

            entity.HasOne(d => d.Exam).WithMany(p => p.Grades)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Grades_Exams");

            entity.HasOne(d => d.Student).WithMany(p => p.Grades)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Grades_Students");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Students__3214EC078ECD8D7D");

            entity.Property(e => e.FullName).HasMaxLength(100);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(true)
                .IsRequired();

            entity.HasMany(c => c.Students)
                .WithMany(s => s.Courses)
                .UsingEntity(
                    "StudentCourse",
                    r => r.HasOne(typeof(Student)).WithMany()
                        .HasForeignKey("StudentId").HasPrincipalKey(nameof(Student.Id)),
                    l => l.HasOne(typeof(Course)).WithMany().HasForeignKey("CourseId")
                        .HasPrincipalKey(nameof(Course.Id)),
                    j => j.HasKey("StudentId", "CourseId")

                );
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
