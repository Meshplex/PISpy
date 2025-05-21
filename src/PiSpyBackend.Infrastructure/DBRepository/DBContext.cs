using PiSpyBackend.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using Npgsql;
using PiSpyBackend.Domain.Models;

namespace PiSpyBackend.Infrastructure
{
    public class AppDbContext : DbContext
    {
        private readonly string _dbUsername = "pispyuser";
        private readonly string _dbPassword = "jnoriwhvoi345345.35,3.4";
        private readonly string _dbName = "pispydatabase";
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Key> keys { get; set; }
        public DbSet<PictureDTO> Pictures { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connBuilder = new NpgsqlConnectionStringBuilder
            {
                Host = "localhost",
                Port = 5432,
                Username = _dbUsername,
                Password = _dbPassword,
                Database = _dbName,
            };

            optionsBuilder.UseNpgsql(
                connBuilder.ConnectionString,
                options => options.EnableRetryOnFailure())
            .EnableDetailedErrors();
        }
    }
}