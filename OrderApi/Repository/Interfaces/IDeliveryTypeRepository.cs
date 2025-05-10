using Library.Common;
using Library.Interfaces;
using OrderApi.Models;

namespace OrderAPI.Repository
{
    public interface IDeliveryTypeRepository : IManageable<DeliveryType>
    {
        // Renamed from "GetAllPaginatedAsync" to "GetAllAsync"
        Task<PaginatedResult<DeliveryType>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            DeliverySort? sort
        );
        // Task DeleteAsync(Guid id);
    }
}
