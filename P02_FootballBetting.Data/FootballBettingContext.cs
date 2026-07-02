using Microsoft.EntityFrameworkCore;
using P02_FootballBetting.Data.Models;

namespace P02_FootballBetting.Data
{
    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext()
        {
        }

        public FootballBettingContext(DbContextOptions<FootballBettingContext> options) : base(options)
        {

        }

        public virtual DbSet<Bet> Bets
        {
            get;
            set;
        } = null!;

        public virtual DbSet<Color> Colors
        {
            get; set;
        } = null!;

        public virtual DbSet<Player> Players
        {
            get; set;
        } = null!;

        public virtual DbSet<Country> Countries
        {
            get; set;
        } = null!;

        public virtual DbSet<Game> Games
        {
            get; set;
        }
            = null!;

        public virtual DbSet<Town> Towns
        {
            get; set;
        }
            = null!;

        public virtual DbSet<PlayerStatistic> PlayersStatistics
        {
            get; set;
        }

        public virtual DbSet<Position> Positions
        {
            get; set;
        }
            = null!;

        public virtual DbSet<Team> Teams
        {
            get; set;
        }
            = null!;

        public virtual DbSet<User> Users
        {
            get; set;
        }
            = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(@"Server=PREDATOR\SQLEXPRESS;Database=FootballBetting;Trusted_Connection=True;")
                    .LogTo(Console.WriteLine);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // In this method we are writing the FluentAPI configuration

            modelBuilder.Entity<PlayerStatistic>(entity =>
            {
                // Composite PK is configured in FluentAPI as object
                entity.HasKey(ps => new { ps.GameId, ps.PlayerId });
            });
        }
    }
}
