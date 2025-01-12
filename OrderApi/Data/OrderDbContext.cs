using OrderApi.Models;
using Microsoft.EntityFrameworkCore;
using OrderApi.Data.Configurations;


namespace OrderApi.Data
{
    public class OrderDbContext : DbContext
    {
        internal DbSet<Order> Orders { get; private set; } = null!;
        internal DbSet<DeliveryType> DeliveryTypes { get; private set; } = null!;

        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new DeliveryTypeConfiguration());

            DataSeeder.Seed(modelBuilder);
        }
    }
}
