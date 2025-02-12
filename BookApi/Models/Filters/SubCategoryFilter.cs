using BookApi.Models;

namespace BookAPI.Models.Filters
{
    public class SubCategoryFilter : IFilter<SubCategory>
    {
        public Guid? CategoryId { get; set; }

        public IQueryable<SubCategory> Apply(IQueryable<SubCategory> query)
        {
            if (CategoryId.HasValue)
                query = query.Where(sc => sc.Category.Id == CategoryId.Value);

            return query;
        }
    }



}
