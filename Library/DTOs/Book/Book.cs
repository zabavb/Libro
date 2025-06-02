using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Library.DTOs.Book
{
    public class Book
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
        public string? ImageUrl { get; set; }
        public string? AudioFileUrl { get; set; }
        public string? PdfFileUrl { get; set; }
        public BookFormat Format { get; set; }

        [JsonIgnore]
        public bool HasAudio => !string.IsNullOrEmpty(AudioFileUrl);

        public List<Guid> FeedbackIds { get; set; } = new List<Guid>();
        public List<Guid> SubcategoryIds { get; set; } = new List<Guid>();
    }
}
