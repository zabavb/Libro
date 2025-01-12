using Library.Extensions;
using Library.Interfaces;

namespace UserAPI.Services
{
    public interface ISubscriptionService : IManagable<SubscriptionDto>
    {
        Task<PaginatedResult<SubscriptionDto>> GetAllAsync(int pageNumber, int pageSize, string searchTerm);
    }
}
