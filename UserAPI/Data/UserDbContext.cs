using Microsoft.EntityFrameworkCore;
using UserAPI.Data.Configurations;
using UserAPI.Models;
using UserAPI.Models.Subscription;

namespace UserAPI.Data
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; private set; } = null!;
        public DbSet<Subscription> Subscriptions { get; private set; } = null!;
        public DbSet<UserSubscription> UserSubscriptions { get; private set; } = null!;
        public DbSet<Password> Passwords { get; private set; } = null!;

        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new SubscriptionConfiguration());
            modelBuilder.ApplyConfiguration(new PasswordConfiguration());

            modelBuilder.Entity<UserSubscription>()
                .HasKey(us => new { us.UserId, us.SubscriptionId });

            modelBuilder.Entity<UserSubscription>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserSubscriptions)
                .HasForeignKey(us => us.UserId);

            modelBuilder.Entity<UserSubscription>()
                .HasOne(us => us.Subscription)
                .WithMany(s => s.UserSubscriptions)
                .HasForeignKey(us => us.SubscriptionId);

            modelBuilder.Entity<UserSubscription>()
                .Property(s => s.ExpirationDate)
                .HasColumnType("datetime")
                .IsRequired();

            modelBuilder.Entity<Password>()
                .Property(p => p.PasswordHash)
                .HasColumnType("nvarchar(150)");

            modelBuilder.Entity<Password>()
                .Property(p => p.PasswordSalt)
                .HasColumnType("nvarchar(30)");

            DataSeeder.Seed(modelBuilder);
        }
    }
}