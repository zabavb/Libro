using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
                   .WithMany(sc => sc.Books)
                   .UsingEntity(j => j.ToTable("BookSubCategories"));
            builder.Property(b => b.IsAvaliable)
                   .HasDefaultValue(true);
        }
    }
}
