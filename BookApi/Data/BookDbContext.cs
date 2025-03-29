using BookAPI.Models;
using Microsoft.EntityFrameworkCore;
using Library.Common;

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

        private readonly S3StorageService _storageService;

        public BookDbContext(DbContextOptions<BookDbContext> options, S3StorageService storageService)
            : base(options)
        {
            _storageService = storageService;
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookDbContext).Assembly);

            DataSeeder.Seed(modelBuilder, _storageService).Wait(); 
        }
    }
}