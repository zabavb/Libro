using BookApi.Models;
using Library.Extensions;
using Library.Interfaces;

namespace BookAPI.Repositories
{
    public interface IFeedbackRepository : IManagable<Feedback>
    {
        Task<PaginatedResult<Feedback>> GetAllAsync(int pageNumber, int pageSize);
        Task DeleteAsync(Guid id);
    }
}
