using Microsoft.AspNetCore.Http;

namespace Library.DTOs.Book
{
    public class UpdateBookRequest
    {
        public BookRequest Book { get; set; }
        public Discount? Discount { get; set; }
        
    }
}
