namespace Library.DTOs.Book
{
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

    public class BookFeedbacks
    {
        public int? AvgRating { get; set; }
        public int FeedbackAmount { get; set; }
    }
}
