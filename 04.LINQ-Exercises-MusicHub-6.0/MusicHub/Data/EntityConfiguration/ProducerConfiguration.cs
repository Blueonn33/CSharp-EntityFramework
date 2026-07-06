using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicHub.Data.Models;
using static MusicHub.Common.EntityValidation;

namespace MusicHub.Data.EntityConfiguration
{
    public class ProducerConfiguration : IEntityTypeConfiguration<Producer>
    {
        public void Configure(EntityTypeBuilder<Producer> entity)
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(ProducerNameMaxLength);

            entity.Property(p => p.Pseudonym)
                .IsRequired(false)
                .HasMaxLength(ProducerPseudonymMaxLength);

            entity.Property(p => p.PhoneNumber)
                .IsRequired(false)
                .HasMaxLength(ProducerPhoneNumberMaxLength);
        }
    }
}