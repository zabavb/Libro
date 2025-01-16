using BookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Data
{
    public class BookDbContext : DbContext
    {
        internal DbSet<Book> Books { get; set; } = null!;
        internal DbSet<Category> Categories { get; set; } = null!;
        internal DbSet<Publisher> Publishers { get; set; } = null!;
        internal DbSet<Feedback> Feedbacks { get; set; } = null!;
        internal DbSet<Author> Authors { get; set; } = null!;
        internal DbSet<SubCategory> Subcategories { get; set; } = null!;


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
