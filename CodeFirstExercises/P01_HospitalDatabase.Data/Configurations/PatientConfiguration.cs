using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data.Configurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(p => p.PatientId);

            builder.Property(p => p.FirstName)
                .IsUnicode(true);

            builder.Property(p => p.LastName)
                .IsUnicode(true);

            builder.Property(p => p.Address)
                .IsUnicode(true);

            builder.Property(p => p.Email)
                .IsUnicode(false);
        }
    }
}
