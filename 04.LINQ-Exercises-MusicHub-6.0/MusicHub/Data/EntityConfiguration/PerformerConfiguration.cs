using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicHub.Data.Models;

namespace MusicHub.Data.EntityConfiguration
{
    public class PerformerConfiguration : IEntityTypeConfiguration<Performer>

    {
        public void Configure(EntityTypeBuilder<Performer> entity)
        {
            entity.HasKey(x => x.Id);

            entity.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength()
        }
    }
}
