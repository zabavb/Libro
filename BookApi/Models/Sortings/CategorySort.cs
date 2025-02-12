using BookApi.Models;
using BookAPI.Models;
using Library.Sortings;
using System.Linq.Expressions;

namespace BookAPI.Models.Sortings
{
    public class CategorySort : SortBase<Category>
    {
        public Bool Name { get; set; }
        public Bool SubCategoryCount { get; set; }
        public Bool BookCount { get; set; }

        public override IQueryable<Category> Apply(IQueryable<Category> categories)
        {
            categories = ApplySorting(categories, Name, c => c.Name);

            categories = ApplySorting(categories, SubCategoryCount, c => c.Subcategories.Count);

            categories = ApplySorting(categories, BookCount, c => c.Subcategories.Sum(sc => sc.Books.Count));

            return categories;
        }

        
    }
}
