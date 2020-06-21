using Microsoft.EntityFrameworkCore;

namespace clase_3.Models
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }
    }
}
