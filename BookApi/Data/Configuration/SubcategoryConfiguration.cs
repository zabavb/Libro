using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BookApi.Models;

namespace BookApi.Data.Configuration
{
    internal class SubcategoryConfiguration : IEntityTypeConfiguration<SubCategory>
    {
        public void Configure(EntityTypeBuilder<SubCategory> builder)
        {
            builder.HasKey(sc => sc.Id);

            builder.Property(sc => sc.Id)
                   .HasDefaultValueSql("NEWID()");
                  
            builder.HasOne(sc => sc.Book)
                   .WithMany(b => b.Subcategories)
                   .HasForeignKey(sc => sc.BookId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sc => sc.Category)
                   .WithMany(c => c.Subcategories)
                   .HasForeignKey(sc => sc.CategoryId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
