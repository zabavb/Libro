namespace Library.DTOs.Book
{
    public class Book
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public Guid AuthorId { get; set; }
        public Guid PublisherId { get; set; }
        public Guid CategoryId { get; set; }
        public float Price { get; set; }
        public Language Language { get; set; }
        public DateTime Year { get; set; }
        public string? Description { get; set; }
        public CoverType Cover { get; set; }
        public bool IsAvaliable { get; set; }
        public List<Guid> FeedbackIds { get; set; } = new List<Guid>();
    }
}
