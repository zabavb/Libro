using Library.Common;
using Library.Interfaces;
using UserAPI.Models.Filters;
using UserAPI.Models.Subscription;

namespace UserAPI.Repositories.Interfaces
{
    public interface ISubscriptionRepository : IManageable<Subscription>
    {
        Task<PaginatedResult<Subscription>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm);

        Task SubscribeAsync(Guid subscriptionId, Guid userId);
        Task UnsubscribeAsync(Guid subscriptionId, Guid userId);
        Task<int> GetActiveSubscriptionsCountAsync();
        Task<ICollection<BySubscription>> GetAllForFilterContentAsync();
    }
}