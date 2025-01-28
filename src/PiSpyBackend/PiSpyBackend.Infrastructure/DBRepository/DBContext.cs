using PiSpyBackend.Domain;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;

namespace PiSpyBackend.Infrastructure
{
    public class AppDbContext : DbContext
    {
        private string connectionString = "server=localhost;port=3306;user=root;database=PispyDatabase";
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Key> keys { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                connectionString, 
                ServerVersion.AutoDetect(connectionString),
                options => options.EnableRetryOnFailure())
            .EnableDetailedErrors();    
        }
    }
}