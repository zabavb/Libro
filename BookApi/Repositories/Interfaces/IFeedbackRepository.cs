using BookAPI.Models;
using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using Library.Extensions;
using Library.Interfaces;

namespace BookAPI.Repositories.Interfaces
{
    public interface IFeedbackRepository : IManagable<Feedback>
    {
        Task<PaginatedResult<Feedback>> GetAllAsync(int pageNumber, int pageSize, FeedbackFilter? filter, FeedbackSort? sort);
        Task DeleteAsync(Guid id);
    }
}
