using Library.Common;
using Library.DTOs.UserRelated.Subscription;
using Library.Interfaces;

namespace UserAPI.Services.Interfaces
{
    public interface ISubscriptionService : IManageable<SubscriptionDto, SubscriptionDto>
    {
        Task<PaginatedResult<SubscriptionCardDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm);
    }
}