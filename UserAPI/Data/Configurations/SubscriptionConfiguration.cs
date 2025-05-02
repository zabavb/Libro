using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UserAPI.Models.Subscription;

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
                .HasMaxLength(30)
                .HasColumnType("nvarchar(30)");

            builder.Property(s => s.Price)
                .IsRequired()
                .HasColumnType("money");

            builder.Property(s => s.Subdescription)
                .HasMaxLength(40)
                .HasColumnType("nvarchar(40)");

            builder.Property(s => s.Description)
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");
        }
    }
}