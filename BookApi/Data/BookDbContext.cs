using BookAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Data
{
    public class BookDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Publisher> Publishers { get; set; } = null!;
        public DbSet<Feedback> Feedbacks { get; set; } = null!;
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<SubCategory> Subcategories { get; set; } = null!;

        public DbSet<Discount> Discounts { get; set; } = null!;


        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookDbContext).Assembly);

            DataSeeder.Seed(modelBuilder);
        }



    }
}
