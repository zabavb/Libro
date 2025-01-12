using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BookApi.Models;

namespace BookApi.Data.Configuration
{
    internal class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.Property(f => f.Id)
                   .HasDefaultValueSql("NEWID()");
        }
    }
}
