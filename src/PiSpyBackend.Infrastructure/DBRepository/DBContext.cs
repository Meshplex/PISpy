using PiSpyBackend.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace PiSpyBackend.Infrastructure
{
    public class AppDbContext : DbContext
    {
        private string connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ?? throw new Exception("DATABASE_URL is not set");
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Key> keys { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                connectionString,
                options => options.EnableRetryOnFailure())
            .EnableDetailedErrors();
        }
    }
}