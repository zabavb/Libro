namespace Library.DTOs.Book
{
    public class SubCategory
    {
        public Guid SubCategoryId { get; set; }         
        public Guid BookId { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
    }
}
