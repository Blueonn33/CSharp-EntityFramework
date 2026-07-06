using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicHub.Data.Models;
using static MusicHub.Common.EntityValidation;

namespace MusicHub.Data.EntityConfiguration
{
    public class SongConfiguration : IEntityTypeConfiguration<Song>
    {
        public void Configure(EntityTypeBuilder<Song> entity)
        {
            entity.HasKey(x => x.Id);

            entity.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(SongNameMaxLength);

            entity.Property(s => s.Price)
                .HasColumnType(SongPriceColumnType);

            entity.HasOne(s => s.Writer)
                .WithMany(w => w.Songs)
                .HasForeignKey(s => s.WriterId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(s => s.Album)
                .WithMany(a => a.Songs)
                .HasForeignKey(s => s.AlbumId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}