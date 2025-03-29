namespace BookAPI.Models
{
    public class Author
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Biography { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Citizenship { get; set; }
        internal List<Book> Books { get; set; } = new();

        public Author()
        {
            Name = string.Empty;
            Biography = null;
            DateOfBirth = null;
            Citizenship = null;
            Books = new List<Book>();
        }
    }
}
