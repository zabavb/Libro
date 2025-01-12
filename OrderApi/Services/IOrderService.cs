using Library.Extensions;
using Library.Interfaces;

namespace OrderApi.Services
{
    public interface IOrderService : IManagable<OrderDto>
    {
        Task<PaginatedResult<OrderDto>> GetOrdersAsync(int pageNumber, int pageSize, string searchTerm, Filter? filter);
    }
}
