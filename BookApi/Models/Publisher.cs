namespace BookAPI.Models
{
    public class Publisher
    {
        public Guid Id { get; set; } 
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public List<Book> Books { get; set; } = new();
        public Publisher()
        {
            Name = string.Empty;
            Description = null;
            Books = new List<Book>();
        }
    }

}
