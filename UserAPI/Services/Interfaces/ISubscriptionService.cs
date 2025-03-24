using Library.Common;
using Library.Interfaces;

namespace UserAPI.Services.Interfaces
{
    public interface ISubscriptionService : IManageable<SubscriptionDto>
    {
        Task<PaginatedResult<SubscriptionDto>> GetAllAsync(int pageNumber, int pageSize, string searchTerm);
        Task DeleteAsync(Guid id);
    }
}
