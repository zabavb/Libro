using BookApi.Models;
using BookAPI.Models;
using Library.Sortings;
using System.Linq.Expressions;

namespace BookAPI.Models.Sortings
{
    public class AuthorSort : SortBase<Author>
    {
        public Bool Newest { get; set; }
        public Bool FirstName { get; set; }

        public override IQueryable<Author> Apply(IQueryable<Author> authors)
        {
            authors = ApplySorting(authors, Newest, a => a.DateOfBirth);
            authors = ApplySorting(authors, FirstName, a => a.Name);

            return authors;
        }
    }
}
