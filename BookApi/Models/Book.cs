using Library.DTOs.Book;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookAPI.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public float Price { get; set; }
        public Language Language { get; set; }
        public DateTime Year { get; set; }
        public string Description { get; set; } = null!;
        public CoverType Cover { get; set; }
        public int Quantity { get; set; } = 0;
        //public bool IsAvaliable { get; set; } = true;
        public string? ImageUrl { get; set; }
        public string? AudioFileUrl { get; set; }
        [NotMapped]
        public bool HasAudio => !string.IsNullOrEmpty(AudioFileUrl);
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public Guid PublisherId { get; set; }
        public Publisher Publisher { get; set; } = null!;
        public Guid AuthorId { get; set; }
        public Author Author { get; set; } = null!;
        public Guid? DiscountId { get; set; }
        public Discount? Discount { get; set; }
        public List<Feedback> Feedbacks { get; set; } = new();
        public List<SubCategory> Subcategories { get; set; } = new();

        public Book()
        {
            Title = string.Empty;
            AuthorId = Guid.Empty;
            Price = 0;
            PublisherId = Guid.Empty;
            Language = Language.OTHER;
            Year = DateTime.MinValue;
            CategoryId = Guid.Empty;
            Description = string.Empty;
            Cover = CoverType.OTHER;
            Quantity = 0;
            //IsAvaliable = true;
            Feedbacks = new List<Feedback>();
            Subcategories = new List<SubCategory>();
            ImageUrl = string.Empty;
            DiscountId = Guid.Empty;
        }
    }
}
