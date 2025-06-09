namespace Library.DTOs.Book
{
    public class BookDetails
    {
        public Guid BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public float Price { get; set; }
        public Language Language { get; set; }
        public DateTime Year { get; set; }
        public string? Description { get; set; }
        public CoverType Cover { get; set; }
        public int Quantity { get; set; } = 0;
        public string? ImageUrl { get; set; }
        public bool hasDigital { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string AuthorDescription { get; set; } = string.Empty;
        public string? AuthorImageUrl { get; set; }
        public string PublisherName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public List<string> Subcategories { get; set; } = new List<string>();
        public List<FeedbackCard> LatestFeedback { get; set; } = new List<FeedbackCard>();
        public BookFeedbacks BookFeedbacks { get; set; }
    }

    public class BookCard
    {
        public Guid BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public float Price { get; set; }
        public bool IsAvailable { get; set; }
        public string? ImageUrl { get; set; }
        public string? CategoryName { get; set; } = string.Empty;
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public BookFeedbacks? Rating { get; set; }
    }

    public class BookLibraryItem
    {
        public string Title { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public string? PdfFileUrl { get; set; }
        public string? AudioUrl { get; set; }
    }

    public class BookFeedbacks
    {
        public int? AvgRating { get; set; }
        public int FeedbackAmount { get; set; }
    }
}
