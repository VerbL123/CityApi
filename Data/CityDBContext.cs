using CityApi.Models;
using FirtsApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CityApi.Data
{
    public class CityDBContext : DbContext
    {
        public CityDBContext(DbContextOptions options)
            : base(options)
        {
            if (!Database.GetAppliedMigrations().Any())
            {
                Database.Migrate();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Resident> Residents { get; set; }
        public DbSet<Home> Homes { get; set; }
        public DbSet<Apartment> Apartments { get; set; }

        //public DbSet<User> Users { get; set; }

       // public DbSet<UserAuth> userAuths { get; set; }
    }
}

