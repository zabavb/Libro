using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Library.DTOs.Book
{
    public class BookRequest
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
        public IFormFile? Image { get; set; }
        public IFormFile? Audio { get; set; }
        public IFormFile? PDF { get; set; }

        public List<Guid> SubcategoryIds { get; set; } = new List<Guid>();
    }
}
