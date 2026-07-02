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
                    .UseSqlServer(@"Server=PREDATOR\SQLEXPRESS;Database=FootballBetting;Trusted_Connection=True;");
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

            modelBuilder.Entity<Team>(entity =>
            {
                entity
                    .HasOne(t => t.SecondaryKitColor)
                    .WithMany(c => c.SecondaryKitTeams)
                    .HasForeignKey(t => t.SecondaryKitColorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(t => t.PrimaryKitColor)
                    .WithMany(c => c.PrimaryKitTeams)
                    .HasForeignKey(t => t.PrimaryKitColorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity
                    .HasOne(g => g.HomeTeam)
                    .WithMany(t => t.HomeGames)
                    .HasForeignKey(g => g.HomeTeamId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity
                    .HasOne(g => g.AwayTeam)
                    .WithMany(t => t.AwayGames)
                    .HasForeignKey(g => g.AwayTeamId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
