using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicHub.Data.Models;
using static MusicHub.Common.EntityValidation;

namespace MusicHub.Data.EntityConfiguration
{
    public class AlbumConfiguration : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> entity)
        {
            entity.HasKey(x => x.Id);

            entity.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(AlbumNameMaxLength);

            entity.Property(a => a.ReleaseDate)
                .HasColumnType(AlbumReleaseDateColumnType);

            entity.Ignore(a => a.Price);

            entity.HasOne(a => a.Producer)
                .WithMany(p => p.Albums)
                .HasForeignKey(a => a.ProducerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}