using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicHub.Data.Models;
using static MusicHub.Common.EntityValidation;

namespace MusicHub.Data.EntityConfiguration
{
    public class WriterConfiguration : IEntityTypeConfiguration<Writer>
    {
        public void Configure(EntityTypeBuilder<Writer> entity)
        {
            entity.HasKey(w => w.Id);

            entity.Property(w => w.Name)
                .IsRequired()
                .HasMaxLength(WriterNameMaxLength);

            entity.Property(w => w.Pseudonym)
                .IsRequired(false)
                .HasMaxLength(WriterPseudonymMaxLength);
        }
    }
}