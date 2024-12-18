using backend_quadrafacil.Models.PlanModel;
using Microsoft.EntityFrameworkCore;
using QuadraFacil_backend.Models.Arena;
using QuadraFacil_backend.Models.Arena.Space;
using QuadraFacil_backend.Models.Reserve;
using QuadraFacil_backend.Models.Users;

namespace QuadraFacil_backend.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        //DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<ArenaModel> Arenas { get; set; }
        public DbSet<AdressArena> AdressArenas { get; set; }
        public DbSet<ReserveModel> Reserve { get; set; }
        public DbSet<SpaceModel> Spaces { get; set; }
        public DbSet<PlanModel> Plan { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relacionamento entre Arena e AdressArena
            modelBuilder.Entity<ArenaModel>()
                .HasMany(a => a.AdressArenas)
                .WithOne(ad => ad.Arena)
                .HasForeignKey(ad => ad.ArenaId)
                .OnDelete(DeleteBehavior.Restrict);  // Impede exclusão em cascata

            // Relacionamento entre Arena e Space
            modelBuilder.Entity<SpaceModel>()
                .HasOne(s => s.Arena)
                .WithMany(a => a.Spaces)
                .HasForeignKey(s => s.ArenaId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
