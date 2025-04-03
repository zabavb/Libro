using BookAPI.Models;
using System.Linq.Expressions;
using Library.Sorts;

namespace BookAPI.Models.Sortings
{
    public class SubCategorySort : SortBase<SubCategory>
    {
        public Bool Name { get; set; }
        public Bool BookCount { get; set; }

        public override IQueryable<SubCategory> Apply(IQueryable<SubCategory> subcategories)
        {
            subcategories = ApplySorting(subcategories, Name, sc => sc.Name);
            subcategories = ApplySorting(subcategories, BookCount, sc => sc.Books.Count);

            return subcategories;
        }

       
    }
}
