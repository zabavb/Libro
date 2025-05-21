using Microsoft.AspNetCore.Http;


namespace Library.DTOs.Book
{
    public class UpdateBookRequest
    {

        public Guid BookId { get; set; }
        public string Title { get; set; }
        public Guid AuthorId { get; set; }
        public Guid PublisherId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid DiscountId { get; set; }
        public float Price { get; set; }
        public Language Language { get; set; }
        public DateTime Year { get; set; }
        public string? Description { get; set; }
        public CoverType Cover { get; set; }
        public int Quantity { get; set; } = 0;
        //public bool IsAvaliable { get; set; }
        public string? ImageUrl { get; set; }
        public string? AudioFileUrl { get; set; }

        public IFormFile? Image { get; set; }

        public List<Guid> FeedbackIds { get; set; } = new List<Guid>();
        public List<Guid> SubcategoryIds { get; set; } = new List<Guid>();
        public Discount? Discount { get; set; }
        
    }
}
