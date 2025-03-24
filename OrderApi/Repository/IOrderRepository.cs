using Library.Common;
using Library.Interfaces;
using OrderApi.Models;

public interface IOrderRepository : IManageable<Order>
{
    Task<PaginatedResult<Order>> GetAllPaginatedAsync(int pageNumber, int pageSize, string searchTerm, Filter? filter, Sort? sort);
    Task DeleteAsync(Guid id);
}