using Library.Common;
using Library.DTOs.UserRelated.User;
using Library.Interfaces;
using OrderApi.Models;
using OrderAPI.Models;

public interface IOrderRepository : IManageable<Order, Order>
{
    Task<PaginatedResult<Order>> GetAllPaginatedAsync(int pageNumber, int pageSize, string searchTerm, Filter? filter,
        Sort? sort);
    Task<List<int>> GetOrderCountsForLastThreePeriodsAsync(PeriodType periodType);
}