namespace BookAPI.Models
{
    public class SubCategory
    {
        public Guid Id { get; set; } 
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public Category Category { get; set; } = null!;

        public List<Book> Books { get; set; } = new();


        public SubCategory()
        {
            Name = string.Empty;
            Books = new List<Book>();
            Category = null!;
        }
    }
}
