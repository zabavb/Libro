using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UserAPI.Models;

namespace UserAPI.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.UserId)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            builder.Property(u => u.LastName)
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            builder.Property(u => u.DateOfBirth)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.PhoneNumber)
                .IsRequired()
                .HasMaxLength(15)
                .HasColumnType("nvarchar(15)");

            builder.Property(u => u.Role)
                .IsRequired()
                .HasConversion<string>();

            builder.HasOne(u => u.Password)
                .WithOne()
                .HasForeignKey<User>(u => u.PasswordId)
                .IsRequired();

            /*builder.HasOne(u => u.Subscription)
                .WithMany(s => s.Users)
                .HasForeignKey(u => u.SubscriptionId)
                .IsRequired(false);*/
        }
    }
}
