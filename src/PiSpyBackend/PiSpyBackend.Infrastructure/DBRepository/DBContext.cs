using PiSpyBackend.Domain;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;

namespace PiSpyBackend.Infrastructure
{
    public class AppDbContext : DbContext
    {
        private string connectionString = "server=localhost;database=PispyDatabase;User=efcore;Password=12345";
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