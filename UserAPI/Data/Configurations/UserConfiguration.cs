﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
                .HasColumnType("datetime");

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnType("nvarchar(15)");

            builder.HasIndex(u => u.PhoneNumber)
                .IsUnique();

            builder.Property(u => u.Role)
                .IsRequired()
                .HasConversion<string>();
        }
    }
}