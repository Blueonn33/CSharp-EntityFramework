using Microsoft.EntityFrameworkCore;
using MiniCinemaApp.Data.Models;

namespace MoviesApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; } = null!;

        public DbSet<Watchlist> Watchlists { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // If you later want Fluent API config, put it here.
        }
    }
}
