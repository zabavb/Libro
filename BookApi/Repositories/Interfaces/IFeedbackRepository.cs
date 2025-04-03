using BookAPI.Models;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using Library.Common;
using Library.Interfaces;

namespace BookAPI.Repositories.Interfaces
{
    public interface IFeedbackRepository : IManageable<Feedback,Feedback>
    {
        Task<PaginatedResult<Feedback>> GetAllAsync(int pageNumber, int pageSize, FeedbackFilter? filter, FeedbackSort? sort);
        Task DeleteAsync(Guid id);
    }
}
