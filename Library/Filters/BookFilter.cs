using Library.DTOs.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Filters
{
    public class BookFilter
    {
        public string? Title { get; set; }
        public Guid? AuthorId { get; set; }
        public float? MinPrice { get; set; }
        public float? MaxPrice { get; set; }
        public DateTime? MinYear { get; set; }
        public DateTime? MaxYear { get; set; }
        public Language? Language { get; set; }
        public CoverType? Cover { get; set; }
        public bool? IsAvaliable { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
