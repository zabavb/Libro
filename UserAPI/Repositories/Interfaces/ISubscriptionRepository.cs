using Library.Common;
using Library.Interfaces;
using UserAPI.Models;

namespace UserAPI.Repositories
{
    public interface ISubscriptionRepository : IManageable<Subscription>
    {
        Task<PaginatedResult<Subscription>> GetAllAsync(int pageNumber, int pageSize, string searchTerm);
        Task<IEnumerable<Subscription>> SearchAsync(string searchTerm);
        Task<Subscription?> GetByIdAsync(Guid id);
        Task DeleteAsync(Guid id);
    }
}
