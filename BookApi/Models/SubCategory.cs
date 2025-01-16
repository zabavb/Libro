namespace BookApi.Models
{
    public class SubCategory
    {
        public Guid Id { get; set; } 
        public Guid BookId { get; set; } 
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = null!;

        public Book Book { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
}
