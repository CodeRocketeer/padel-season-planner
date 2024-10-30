using Microsoft.EntityFrameworkCore;
using Padel.Infrastructure.Entities;

namespace Padel.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<SeasonEntity> Seasons { get; set; }
    public DbSet<MatchEntity> Matches { get; set; }
    public DbSet<TeamEntity> Teams { get; set; }
    public DbSet<PlayerEntity> Players { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define Many-to-Many relationship between Team and Player
        modelBuilder.Entity<TeamEntity>()
            .HasMany(t => t.Players)
            .WithMany(p => p.Teams)
            .UsingEntity(j => j.ToTable("TeamPlayers")); // This creates the join table for Teams and Players

        // Define Many-to-Many relationship between Player and Season
        modelBuilder.Entity<PlayerEntity>()
            .HasMany(p => p.Seasons)
            .WithMany(s => s.Players)
            .UsingEntity(j => j.ToTable("PlayerSeasons")); // This creates the join table for Players and Seasons

        // Configure one-to-many relationship between Season and Match
        modelBuilder.Entity<SeasonEntity>()
            .HasMany(s => s.Matches)
            .WithOne(m => m.Season)
            .HasForeignKey(m => m.SeasonId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure one-to-many relationship between Match and Team
        modelBuilder.Entity<MatchEntity>()
            .HasMany(m => m.Teams)
            .WithOne(t => t.Match)
            .HasForeignKey(t => t.MatchId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }


}
