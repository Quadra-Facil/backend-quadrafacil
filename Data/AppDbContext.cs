using Microsoft.EntityFrameworkCore;
using QuadraFacil_backend.API.Models.Users;

namespace QuadraFacil_backend.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Suas DbSets aqui, exemplo:
        public DbSet<User> Users { get; set; }
    }
}
