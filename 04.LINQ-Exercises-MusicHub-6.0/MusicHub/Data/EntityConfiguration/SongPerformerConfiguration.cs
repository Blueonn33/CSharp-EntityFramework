using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicHub.Data.Models;

namespace MusicHub.Data.EntityConfiguration
{
    public class SongPerformerConfiguration : IEntityTypeConfiguration<SongPerformer>

    {
        public void Configure(EntityTypeBuilder<SongPerformer> builder)
        {
            throw new NotImplementedException();
        }
    }
}
