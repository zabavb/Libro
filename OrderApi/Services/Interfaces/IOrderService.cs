using Library.Common;
using Library.DTOs.UserRelated.User;
using Library.Interfaces;

namespace OrderApi.Services
{
    public interface IOrderService : IManageable<OrderDto>
    {
        Task<PaginatedResult<OrderDto>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string searchTerm,
            Filter? filter,
            Sort? sort
        );

        Task<OrderForUserCard?> GetForUserCardAsync(Guid id);
        Task<ICollection<OrderForUserDetails>?> GetAllForUserDetailsAsync(Guid id);
    }
}