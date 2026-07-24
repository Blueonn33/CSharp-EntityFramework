using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data.Configurations
{
    public class VisitationConfiguration : IEntityTypeConfiguration<Visitation>
    {
        public void Configure(EntityTypeBuilder<Visitation> modelBuilder)
        {
            modelBuilder.HasKey(v => v.VisitationId);

            modelBuilder.Property(v => v.Comments)
                .IsUnicode(true);
        }
    }
}
