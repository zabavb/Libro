using Library.Common;
using Library.DTOs.UserRelated.User;
using Library.Interfaces;

namespace OrderApi.Services
{
    public interface IOrderService : IManageable<OrderDto,OrderDto>
    {
        Task<PaginatedResult<OrderCardDto>> GetAllAsync(int pageNumber, int pageSize, string searchTerm, Filter? filter, Sort? sort);
        Task DeleteAsync(Guid id);
        Task<SingleSnippet<OrderCardSnippet>> GetCardSnippetByUserIdAsync(Guid id);
        Task<CollectionSnippet<OrderDetailsSnippet>> GetAllByUserIdAsync(Guid id, int pageNumber = 1);
    }
}
