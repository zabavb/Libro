using Library.Common;
using Library.DTOs.UserRelated.User;
using Library.Interfaces;
using OrderApi.Models;

public interface IOrderRepository : IManageable<Order, Order>
{
    Task<PaginatedResult<Order>> GetAllPaginatedAsync(int pageNumber, int pageSize, string searchTerm, Filter? filter, Sort? sort);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<OrderDetailsSnippet>?> GetAllByUserId(Guid Id);
    Task<OrderCardSnippet?> GetCardSnippetByUserId(Guid Id);
}