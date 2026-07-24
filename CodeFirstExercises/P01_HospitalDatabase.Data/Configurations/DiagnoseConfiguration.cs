using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data.Configurations
{
    public class DiagnoseConfiguration : IEntityTypeConfiguration<Diagnose>
    {
        public void Configure(EntityTypeBuilder<Diagnose> modelBuilder)
        {
            modelBuilder.HasKey(d => d.DiagnoseId);

            modelBuilder.Property(d => d.Name)
                .IsUnicode(true);

            modelBuilder.Property(d => d.Comments)
                .IsUnicode(true);
        }
    }
}
