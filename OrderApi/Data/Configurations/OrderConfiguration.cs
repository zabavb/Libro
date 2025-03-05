using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderApi.Models;
using System.Text.Json;

namespace OrderApi.Data.Configurations
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.OrderId)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.Property(o => o.Address)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnType("nvarchar(255)");

            builder.Property(o => o.Region)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            builder.Property(o => o.City)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            builder.Property(o => o.Price)
                .IsRequired()
                .HasColumnType("float");

            builder.Property(o => o.Status)
                .HasConversion<string>();

            builder.Property(e => e.Books)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => JsonSerializer.Deserialize<Dictionary<Guid, int>>(v, new JsonSerializerOptions())
                );


            builder.HasOne(o => o.DeliveryType)
                .WithMany(dt => dt.Orders)
                .HasForeignKey(o => o.DeliveryTypeId)
                .IsRequired();
        }
    }
}
