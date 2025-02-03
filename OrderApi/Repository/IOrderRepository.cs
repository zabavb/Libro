using Library.Extensions;
using Library.Interfaces;
using OrderApi.Models;

public interface IOrderRepository : IManagable<Order>
{
    Task<PaginatedResult<Order>> GetAllPaginatedAsync(int pageNumber, int pageSize, string searchTerm, Filter? filter, Sort? sort);
}