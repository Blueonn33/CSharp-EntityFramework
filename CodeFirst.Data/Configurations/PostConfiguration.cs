using CodeFirst.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeFirst.Data.Configurations
{
    internal class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(p => p.CreatedOn)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(p => p.UpdatedOn)
                .IsRequired()
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}
