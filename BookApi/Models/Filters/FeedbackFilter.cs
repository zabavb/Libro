using BookAPI.Models;

namespace BookAPI.Models.Filters
{

    //public class FeedbackFilter
    //{
    //    public int? Rating { get; set; } 
    //    public DateTime? MinPublicationDate { get; set; }
    //    public DateTime? MaxPublicationDate { get; set; } 
    //    public bool? IsPurchasedByReviewer { get; set; }
    //}

    public class FeedbackFilter : IFilter<Feedback>
    {
        public int? Rating { get; set; }
        public DateTime? MinPublicationDate { get; set; }
        public DateTime? MaxPublicationDate { get; set; }
        public bool? IsPurchasedByReviewer { get; set; }

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

            return query;
        }
    }

}
