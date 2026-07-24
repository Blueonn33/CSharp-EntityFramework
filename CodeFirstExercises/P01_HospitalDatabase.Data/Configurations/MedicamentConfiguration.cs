using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data.Configurations
{
    public class MedicamentConfiguration : IEntityTypeConfiguration<Medicament>
    {
        public void Configure(EntityTypeBuilder<Medicament> modelBuilder)
        {
            modelBuilder.HasKey(m => m.MedicamentId);

            modelBuilder.Property(m => m.Name)
                .IsUnicode(true);
        }
    }
}
