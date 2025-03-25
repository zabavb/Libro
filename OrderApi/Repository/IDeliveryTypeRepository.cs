using Library.Common;
using Library.Interfaces;
using OrderApi.Models;

namespace OrderAPI.Repository
{
    public interface IDeliveryTypeRepository : IManageable<DeliveryType>
    {
        Task<PaginatedResult<DeliveryType>> GetAllPaginatedAsync(int pageNumber, int pageSize, string? searchTerm, DeliverySort? sort);
        Task DeleteAsync(Guid id);
    }
}
