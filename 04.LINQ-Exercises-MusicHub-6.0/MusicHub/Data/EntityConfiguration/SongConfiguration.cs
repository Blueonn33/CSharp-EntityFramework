using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicHub.Data.Models;

namespace MusicHub.Data.EntityConfiguration
{
    public class SongConfiguration : IEntityTypeConfiguration<Song>
    {
        public void Configure(EntityTypeBuilder<Song> entity)
        {
            entity.HasKey(x => x.Id);

            entity.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(20);


        }
    }
}
