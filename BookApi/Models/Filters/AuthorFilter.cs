using BookApi.Models;

namespace BookAPI.Models.Filters
{
    public class AuthorFilter : IFilter<Author>
    {
        public DateTime? MinBirthDate { get; set; }
        public DateTime? MaxBirthDate { get; set; }

        public IQueryable<Author> Apply(IQueryable<Author> query)
        {
            if (MinBirthDate.HasValue)
                query = query.Where(a => a.DateOfBirth >= MinBirthDate.Value);

            if (MaxBirthDate.HasValue)
                query = query.Where(a => a.DateOfBirth <= MaxBirthDate.Value);

            return query;
        }
    }
}
