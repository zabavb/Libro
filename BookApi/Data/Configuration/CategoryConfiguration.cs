using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BookAPI.Models;

namespace BookAPI.Data.Configuration
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.Id)
                   .HasDefaultValueSql("NEWID()");
           
            builder.HasMany(c => c.Subcategories)
                   .WithOne(sc => sc.Category)
                   .HasForeignKey(sc => sc.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

