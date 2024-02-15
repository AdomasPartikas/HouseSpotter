using HouseSpotter.Models;
using Microsoft.EntityFrameworkCore;

namespace HouseSpotter.Data
{
    public class HouseContext : DbContext
    {
        public HouseContext(DbContextOptions<HouseContext> options) : base(options)
        {

        }

        public DbSet<HouseDTO> House => Set<HouseDTO>();
    }
}