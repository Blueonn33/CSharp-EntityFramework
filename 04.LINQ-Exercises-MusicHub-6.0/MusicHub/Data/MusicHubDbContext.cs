using MusicHub.Data.EntityConfiguration;
using MusicHub.Data.Models;

namespace MusicHub.Data
{
    using Microsoft.EntityFrameworkCore;

    public class MusicHubDbContext : DbContext
    {
        public MusicHubDbContext()
        {
        }

        public MusicHubDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Album> Albums
        {
            get;
            set;
        } = null!;

        public virtual DbSet<Performer> Performers
        {
            get;
            set;
        } = null!;

        public virtual DbSet<Producer> Producers
        {
            get; set;
        } = null!;

        public virtual DbSet<Song> Songs
        {
            get; set;
        } = null!;

        public virtual DbSet<SongPerformer> SongsPerformers
        {
            get;
            set;
        } = null!;

        public virtual DbSet<Writer> Writers
        {
            get;
            set;
        } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        // I. При малко на брой entity configurations
        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    builder.ApplyConfiguration(new AlbumConfiguration());
        //    builder.ApplyConfiguration(new SongConfiguration());
        //    builder.ApplyConfiguration(new WriterConfiguration());
        //    builder.ApplyConfiguration(new PerformerConfiguration());
        //    builder.ApplyConfiguration(new SongPerformerConfiguration());
        //}

        // II. При по-голям брой entity configurations
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.ApplyConfigurationsFromAssembly(typeof(AlbumConfiguration).Assembly);   // избираме случайна конфигурация, за да вземем проекта, в който се намират всички
        }
    }
}