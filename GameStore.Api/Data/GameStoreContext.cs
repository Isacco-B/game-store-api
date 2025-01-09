using GameStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Genre>()
            .HasData(
                new { Id = 1, Name = "Action" },
                new { Id = 2, Name = "Adventure" },
                new { Id = 3, Name = "RPG" },
                new { Id = 4, Name = "Strategy" },
                new { Id = 5, Name = "Sports" }
            );
    }
}
