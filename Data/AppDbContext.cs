using Microsoft.EntityFrameworkCore;
using QuadraFacil_backend.Models.Arena;
using QuadraFacil_backend.Models.Users;
using System.Net;

namespace QuadraFacil_backend.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // Suas DbSets aqui, exemplo:
    public DbSet<User> Users { get; set; }
    public DbSet<ArenaModel> Arenas { get; set; }
    public DbSet<AdressArena> AdressArenas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuração do relacionamento
        modelBuilder.Entity<ArenaModel>()
            .HasMany(a => a.AdressArenas)
            .WithOne(ad => ad.Arena)
            .HasForeignKey(ad => ad.ArenaId);
    }
}
