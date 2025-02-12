using BookApi.Models;
using BookAPI.Models;
using Library.Sortings;
using System.Linq.Expressions;

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
