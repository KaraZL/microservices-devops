using GeneralStore.Models;
using Microsoft.EntityFrameworkCore;

namespace GeneralStore.API.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<Course> Course { get; set; }
    }
}
