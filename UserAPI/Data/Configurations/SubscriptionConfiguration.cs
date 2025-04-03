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

            builder.Property(s => s.ExpirationDays)
                .IsRequired();

            builder.Property(s => s.Price)
                .IsRequired()
                .HasColumnType("money");

            builder.Property(s => s.Description)
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            builder.HasOne(u => u.User)
                .WithOne()
                .HasForeignKey<User>(u => u.SubscriptionId)
                .IsRequired();
        }
    }
}