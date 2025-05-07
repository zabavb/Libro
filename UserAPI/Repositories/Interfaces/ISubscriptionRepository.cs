using Library.Common;
using Library.DTOs.UserRelated.User;
using Library.Interfaces;
using UserAPI.Models.Subscription;

namespace UserAPI.Repositories.Interfaces
{
    public interface ISubscriptionRepository : IManageable<Subscription, Subscription>
    {
        Task<PaginatedResult<Subscription>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm);
        // Task<CollectionSnippet<SubscriptionDetailsSnippet>> GetAllByUserIdAsync(Guid id);
        Task SubscribeAsync(Guid subscriptionId, Guid userId);
        Task UnsubscribeAsync(Guid subscriptionId, Guid userId);
        Task<int> GetActiveSubscriptionsCountAsync();
    }
}