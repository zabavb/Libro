using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UserAPI.Models;

namespace UserAPI.Data.Configurations
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.Property(s => s.SubscriptionId)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.Property(s => s.Title)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            builder.Property(s => s.Description)
                .HasMaxLength(1000)
                .HasColumnType("nvarchar(1000)");

            builder.Property(s => s.EndDate)
                .IsRequired()
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE() + 1");

            /*builder.HasMany(s => s.Users)
                .WithOne(u => u.Subscription)
                .HasForeignKey(u => u.SubscriptionId);*/
        }
    }
}
