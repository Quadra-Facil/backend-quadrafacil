using Microsoft.EntityFrameworkCore;
using QuadraFacil_backend.Models.Arena;
using QuadraFacil_backend.Models.Arena.Space;
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

    public DbSet<SpaceModel> Spaces { get; set; }   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuração do relacionamento
        modelBuilder.Entity<ArenaModel>()
            .HasMany(a => a.AdressArenas)
            .WithOne(ad => ad.Arena)
            .HasForeignKey(ad => ad.ArenaId);

        // Configuração do relacionamento entre Arena e Space
        modelBuilder.Entity<SpaceModel>()
            .HasOne(s => s.Arena)  // Cada Space tem uma Arena
            .WithMany(a => a.Spaces)  // Uma Arena pode ter muitos Spaces
            .HasForeignKey(s => s.ArenaId);  // Chave estrangeira em Space
    }
}
