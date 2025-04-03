using BookAPI.Models;
using Library.Interfaces;

namespace BookAPI.Models.Filters
{

    public class FeedbackFilter : IFilter<Feedback>
    {
        public int? Rating { get; set; }
        public DateTime? MinPublicationDate { get; set; }
        public DateTime? MaxPublicationDate { get; set; }
        public bool? IsPurchasedByReviewer { get; set; }

        public Guid userId { get; set; }

        public IQueryable<Feedback> Apply(IQueryable<Feedback> query)
        {
            if (Rating.HasValue)
                query = query.Where(b => b.Rating == Rating.Value);

            if (MinPublicationDate.HasValue)
                query = query.Where(b => b.Date >= MinPublicationDate.Value);

            if (MaxPublicationDate.HasValue)
                query = query.Where(b => b.Date <= MaxPublicationDate.Value);

            if (IsPurchasedByReviewer.HasValue)
                query = query.Where(b => b.IsPurchased == IsPurchasedByReviewer.Value);

            if(!userId.Equals(Guid.Empty))
                query = query.Where(b => b.UserId == userId);

            return query;
        }
    }

}
