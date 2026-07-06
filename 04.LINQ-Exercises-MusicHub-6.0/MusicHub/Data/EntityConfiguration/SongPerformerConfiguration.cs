using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicHub.Data.Models;

namespace MusicHub.Data.EntityConfiguration
{
    public class SongPerformerConfiguration : IEntityTypeConfiguration<SongPerformer>

    {
        public void Configure(EntityTypeBuilder<SongPerformer> entity)
        {
            entity.HasKey(sp => new
            {
                sp.SongId,
                sp.PerformerId
            });

            entity.HasOne(sp => sp.Song)
                .WithMany(s => s.SongPerformers)
                .HasForeignKey(sp => sp.SongId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(sp => sp.Performer)
                .WithMany(p => p.PerformerSongs)
                .HasForeignKey(sp => sp.PerformerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}