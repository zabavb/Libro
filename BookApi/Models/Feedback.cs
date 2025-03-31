namespace BookAPI.Models
{
    public class Feedback
    {
        public Guid Id { get; set; } 
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; } 
        public DateTime Date { get; set; }
        public bool IsPurchased { get; set; } 
        public Book Book { get; set; } = null!;
        public Feedback()
        {
            Comment = null;
            Rating = 0;
            Date = DateTime.MinValue;
            IsPurchased = false;
            Book = null!;
            UserId = Guid.Empty;
        }
    }
}
