namespace Library.DTOs.Book
{
    public class UpdateBookRequest
    {
        public Book Book { get; set; }
        public Discount? Discount { get; set; }
    }
}
