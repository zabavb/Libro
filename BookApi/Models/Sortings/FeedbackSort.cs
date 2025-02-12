using BookApi.Models;
using BookAPI.Models;
using Library.Sortings;
using System.Linq.Expressions;

namespace BookAPI.Models.Sortings
{
    public class FeedbackSort : SortBase<Feedback>
    {
        public Bool Newest { get; set; } 
        public Bool Rating { get; set; } 

        public override IQueryable<Feedback> Apply(IQueryable<Feedback> feedbacks)
        {
            feedbacks = ApplySorting(feedbacks, Newest, f => f.Date);
            feedbacks = ApplySorting(feedbacks, Rating, f => f.Rating);

            return feedbacks;
        }

    }
}
