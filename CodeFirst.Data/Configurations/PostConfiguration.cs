using CodeFirst.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeFirst.Data.Configurations
{
    internal class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .Property(p => p.CreatedOn)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Post>()
                .Property(p => p.UpdatedOn)
                .IsRequired()
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}
