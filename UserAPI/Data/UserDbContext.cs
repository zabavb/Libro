using Microsoft.EntityFrameworkCore;
using UserAPI.Data.Configurations;
using UserAPI.Models;

namespace UserAPI.Data
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; private set; } = null!;
        public DbSet<Password> Passwords { get; private set; } = null!;
        public DbSet<Subscription> Subscriptions { get; private set; } = null!;

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new PasswordConfiguration());
            modelBuilder.ApplyConfiguration(new SubscriptionConfiguration());

            modelBuilder.Entity<User>()
                .HasOne(u => u.Subscription) 
                .WithOne(s => s.User)
                .HasForeignKey<Subscription>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
