using BookAPI.Models;
using System.Linq.Expressions;
using Library.Sorts;

namespace BookAPI.Models.Sortings
{
    public class PublisherSort : SortBase<Publisher>
    {
        public Bool Name { get; set; }

        public override IQueryable<Publisher> Apply(IQueryable<Publisher> publishers)
        {
            publishers = ApplySorting(publishers, Name, p => p.Name);

            return publishers;
        }

        
    }
}
