using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using BookAPI.Models;

namespace BookAPI.Data.Configuration
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
