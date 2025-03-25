using Microsoft.EntityFrameworkCore;
using BookAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookAPI.Data.Configuration
{
    public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.Property(d => d.DiscountId)
                   .HasDefaultValueSql("NEWID()");
        }
    }
}
