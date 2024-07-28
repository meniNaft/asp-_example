using Microsoft.EntityFrameworkCore;
using asp__example.Models;

namespace asp__example.DAL
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Friend> Friends { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}

