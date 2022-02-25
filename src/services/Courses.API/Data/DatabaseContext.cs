using Microsoft.EntityFrameworkCore;
using Courses.API.Models;

namespace Courses.API.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<Course> Course { get; set; }
    }
}
