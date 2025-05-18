using Amazon.S3.Model;
using BookAPI.Models;
using Library.DTOs.Book;
using Library.Interfaces;

namespace BookAPI.Models.Filters
{
    public class BookFilter : IFilter<Book>
    {
        public string? Title { get; set; }
        public Guid? AuthorId { get; set; }     
        public float? MinPrice { get; set; }
        public float? MaxPrice { get; set; }
        public DateTime? MinYear { get; set; }
        public DateTime? MaxYear { get; set; }
        public Language? Language { get; set; }
        public CoverType? Cover { get; set; }
        //public bool IsAvaliable { get; set; }
        public bool? HasAudio { get; set; }
        public int Quantity { get; set; }
        public Guid? CategoryId { get; set; }

        public IQueryable<Book> Apply(IQueryable<Book> books)
        {
            if (!string.IsNullOrWhiteSpace(Title))
                books = books.Where(b => b.Title.ToLower().Contains(Title.ToLower()));


            if (AuthorId.HasValue)
                books = books.Where(b => b.AuthorId == AuthorId.Value);

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

            if(Quantity > 0)
                books = books.Where(b => b.Quantity >= Quantity);

            //if (IsAvaliable)
            //    books = books.Where(b => b.IsAvaliable == IsAvaliable);

            if (CategoryId.HasValue)
                books = books.Where(b => b.CategoryId == CategoryId.Value);

            if (HasAudio == true)
                books = books.Where(b => !string.IsNullOrEmpty(b.AudioFileUrl));

            return books;
        }
      
    }
}
