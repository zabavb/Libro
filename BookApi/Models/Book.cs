using Library.DTOs.Book;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookAPI.Models
{
    public class Book
    {
        public Guid Id { get; set; } 
        public string Title { get; set; } = null!;
        public Guid AuthorId { get; set; } 
        public float Price { get; set; }
        public Guid PublisherId { get; set; } 
        public Language Language { get; set; }
        public DateTime Year { get; set; }
        public Guid CategoryId { get; set; } 
        public string Description { get; set; } = null!;
        public CoverType Cover { get; set; }
<<<<<<< HEAD
        public bool IsAvaliable { get; set; } = true; 
        public Discount DiscountId { get; set; }

=======
        public bool IsAvaliable { get; set; } = true;
        public string? ImageUrl { get; set; }
        public string? AudioFileUrl { get; set; }
        [NotMapped]
        public bool HasAudio => !string.IsNullOrEmpty(AudioFileUrl);
>>>>>>> f105b296e3830501faae87360aeaf508b350e1b6
        public Category Category { get; set; } = null!;
        public Publisher Publisher { get; set; } = null!;
        public Author Author { get; set; } = null!;
        public List<Feedback> Feedbacks { get; set; } = new();
        public List<SubCategory> Subcategories { get; set; } = new();
        public Discount Discount { get; set; } = null!;

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
            IsAvaliable = true;
            Feedbacks = new List<Feedback>();
            Subcategories = new List<SubCategory>();
            ImageUrl = string.Empty;
        }
    }
}
