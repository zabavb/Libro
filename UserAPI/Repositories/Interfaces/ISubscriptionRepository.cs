using Library.Common;
using Library.Interfaces;
using UserAPI.Models;

namespace UserAPI.Repositories.Interfaces
{
    public interface ISubscriptionRepository : IManageable<Subscription, Subscription>
    {
        Task<PaginatedResult<Subscription>> GetAllAsync(int pageNumber, int pageSize, string searchTerm);
    }
}