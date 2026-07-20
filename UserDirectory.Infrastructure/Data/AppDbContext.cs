using Microsoft.EntityFrameworkCore;
using UserDirectory.Domain.Models;

namespace UserDirectory.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();

        // Optional: configure model if needed
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // ensure User config matches migrations
            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(e => e.Id);
                b.Property(e => e.Name).IsRequired().HasMaxLength(100);
                b.Property(e => e.Email).IsRequired();
                b.Property(e => e.Phone);
            });
        }
    }
}
