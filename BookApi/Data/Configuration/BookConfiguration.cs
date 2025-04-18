using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookAPI.Models;

namespace BookAPI.Data.Configuration
{
    internal class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(b => b.Id)
                   .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.HasMany(b => b.Subcategories)
                   .WithMany(sc => sc.Books)
                   .UsingEntity(j => j.ToTable("BookSubCategories"));
            builder.HasMany(b => b.Subcategories)
              .WithMany(sc => sc.Books)
              .UsingEntity<Dictionary<string, object>>(
                  "BookSubCategory",
                  j => j.HasOne<SubCategory>().WithMany().HasForeignKey("SubCategoryId"),
                  j => j.HasOne<Book>().WithMany().HasForeignKey("BookId")
              );
            builder.Property(b => b.Quantity)
                   .HasDefaultValue(0);
            //builder.Property(b => b.IsAvaliable)
            //       .HasDefaultValue(true);

            builder
                .HasOne(b => b.Discount)
                .WithOne()
                .HasForeignKey<Discount>(d => d.BookId)
                .IsRequired(false);
        }
    }
}
