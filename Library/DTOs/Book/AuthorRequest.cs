using Microsoft.AspNetCore.Http;

namespace Library.DTOs.Book
{
    public class AuthorRequest
    {
        public Guid AuthorId { get; set; }
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Biography { get; set; }
        public string? Citizenship { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }

    }

}
