using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OrderApi.Models;

namespace OrderApi.Data.Configurations
{
    internal class DeliveryTypeConfiguration : IEntityTypeConfiguration<DeliveryType>
    {
        public void Configure(EntityTypeBuilder<DeliveryType> builder)
        {
            builder.Property(d => d.DeliveryId)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.Property(d => d.ServiceName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");
        }
    }
}
