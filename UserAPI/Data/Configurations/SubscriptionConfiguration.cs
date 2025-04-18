﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UserAPI.Models;
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
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            builder.Property(s => s.Price)
                .HasColumnType("money")
                .IsRequired();

            builder.Property(s => s.Description)
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");
        }
    }
}