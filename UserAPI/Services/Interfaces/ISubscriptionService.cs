using Library.Common;
using Library.DTOs.UserRelated.Subscription;
using Library.Interfaces;
using UserAPI.Models.Filters;
using UserAPI.Models.Subscription;

namespace UserAPI.Services.Interfaces
{
    public interface ISubscriptionService : IManageable<SubscriptionDto, SubscriptionDto>
    {
        Task<PaginatedResult<SubscriptionCardDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm);
        Task SubscribeAsync(SubscribeRequest request);
        Task UnsubscribeAsync(SubscribeRequest request);
        Task<ICollection<BySubscription>> GetAllForFilterContentAsync();
    }
}