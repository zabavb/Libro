using Library.Common;
using Library.DTOs.UserRelated.User;
using Library.Interfaces;
using OrderApi.Models;

public interface IOrderRepository : IManageable<Order>
{
    // Renamed from "GetAllPaginatedAsync" to "GetAllAsync"
    Task<PaginatedResult<Order>> GetAllAsync(
        int pageNumber,
        int pageSize,
        string searchTerm,
        Filter? filter,
        Sort? sort);

    Task<OrderForUserCard> GetForUserCardAsync(Guid userId);
    Task<ICollection<OrderForUserDetails>> GetAllForUserDetailsAsync(Guid userId);
}