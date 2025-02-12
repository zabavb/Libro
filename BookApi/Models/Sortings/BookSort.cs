using BookAPI.Models;
using Library.Sortings;
using System.Linq.Expressions;

namespace BookAPI.Models.Sortings
{
    public class BookSort : SortBase<Book>
    {
        public Bool Newest { get; set; }
        public Bool Alphabetical { get; set; }
        public Bool Price { get; set; }
        public Bool FeedbackCount { get; set; }

        public override IQueryable<Book> Apply(IQueryable<Book> books)
        {
            books = ApplySorting(books, Newest, b => b.Year);
            books = ApplySorting(books, Alphabetical, b => b.Title);
            books = ApplySorting(books, Price, b => b.Price);
            books = ApplySorting(books, FeedbackCount, b => b.Feedbacks.Count);

            return books;
        }

    }
}
