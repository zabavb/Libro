using Amazon.S3.Model;
using BookAPI.Models;
using Library.DTOs.Book;
using Library.Interfaces;

namespace BookAPI.Models.Filters
{
    public class BookFilter : IFilter<Book>
    {
        public Guid? AuthorId { get; set; }     
        public Guid? PublisherId { get; set; }
        public float? MinPrice { get; set; }
        public float? MaxPrice { get; set; }
        public DateTime? MinYear { get; set; }
        public DateTime? MaxYear { get; set; }
        public Language? Language { get; set; }
        public CoverType? Cover { get; set; }
        public bool? HasAudio { get; set; }
        public bool? HasDigital { get; set; }
        public bool? Physical { get; set; }
        public bool? Available { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? SubcategoryId { get; set; }

        public IQueryable<Book> Apply(IQueryable<Book> books)
        {
            if (AuthorId.HasValue)
                books = books.Where(b => b.AuthorId == AuthorId.Value);

            if (PublisherId.HasValue)
                books = books.Where(b => b.PublisherId == PublisherId.Value);

            if (MinPrice.HasValue)
                books = books.Where(b => b.Price >= MinPrice.Value);

            if (MaxPrice.HasValue)
                books = books.Where(b => b.Price <= MaxPrice.Value);

            if (MinYear.HasValue)
                books = books.Where(b => b.Year >= MinYear.Value);

            if (MaxYear.HasValue)
                books = books.Where(b => b.Year <= MaxYear.Value);

            if (Language.HasValue)
                books = books.Where(b => b.Language.ToString() == Language.Value.ToString());

            if (Cover.HasValue)
                books = books.Where(b => b.Cover.ToString() == Cover.Value.ToString());

            if(Available.HasValue)
                books = books.Where(b => (b.Quantity > 0) == Available.Value);

            if (CategoryId.HasValue)
                books = books.Where(b => b.CategoryId == CategoryId.Value);

/*            if (Physical == true)
                books = books.Where(b => string.IsNullOrEmpty(b.AudioFileUrl) && string.IsNullOrEmpty(b.PdfFileUrl));*/

            if (HasAudio == true)
                books = books.Where(b => !string.IsNullOrEmpty(b.AudioFileUrl));

            if (HasDigital == true)
                books = books.Where(b => !string.IsNullOrEmpty(b.PdfFileUrl));

            if (SubcategoryId.HasValue)
                books = books.Where(b => b.Subcategories.Where(s => s.Id == SubcategoryId.Value).Any());

            return books;
        }
      
    }
}
