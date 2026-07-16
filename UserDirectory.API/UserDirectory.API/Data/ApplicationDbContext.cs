using Microsoft.EntityFrameworkCore;
using UserDirectory.API.Models;

namespace UserDirectory.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
    }
}
