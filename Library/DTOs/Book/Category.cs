namespace Library.DTOs.Book
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public List<Guid> SubcategoryIds { get; set; } = new List<Guid>();

    }
}
