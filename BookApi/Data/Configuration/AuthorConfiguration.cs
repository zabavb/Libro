using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BookApi.Models;

namespace BookApi.Data.Configuration
{
    internal class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.Property(a => a.Id)
                   .HasDefaultValueSql("NEWID()");
        }
    }
}
