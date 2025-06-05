using OrderApi.Models;
using Microsoft.EntityFrameworkCore;
using OrderApi.Data.Configurations;

namespace OrderApi.Data
{
    public class OrderDbContext : DbContext
    {
        public DbSet<Order> Orders { get; private set; } = null!;
        public DbSet<DeliveryType> DeliveryTypes { get; private set; } = null!;

        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new DeliveryTypeConfiguration());

            // One-to-many DeliveryType -> Orders
            modelBuilder.Entity<DeliveryType>()
                .HasMany(dt => dt.Orders)
                .WithOne(o => o.DeliveryType)
                .HasForeignKey(o => o.DeliveryTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DeliveryType>()
                .HasKey(d => d.DeliveryId);

            DataSeeder.Seed(modelBuilder);
        }
    }
}
