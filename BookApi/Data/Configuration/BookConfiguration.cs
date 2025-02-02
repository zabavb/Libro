﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BookApi.Models;

namespace BookApi.Data.Configuration
{
    internal class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(b => b.Id)
                   .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.HasMany(b => b.Subcategories)
                   .WithOne(sc => sc.Book)
                   .HasForeignKey(sc => sc.BookId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
