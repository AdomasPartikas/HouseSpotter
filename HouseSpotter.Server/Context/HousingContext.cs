using Microsoft.EntityFrameworkCore;
using HouseSpotter.Server.Models;

namespace HouseSpotter.Server.Context
{
    public class HousingContext : DbContext
    {
        public HousingContext(DbContextOptions<HousingContext> options) : base(options)
        {
        }

        public DbSet<Housing> Housings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Scrape> Scrapes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Housing>().ToTable("Housing");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Scrape>().ToTable("Scrape");
        }
    }
}