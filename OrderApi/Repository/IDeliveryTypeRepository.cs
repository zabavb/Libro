using Library.Extensions;
using Library.Interfaces;
using OrderApi.Models;

namespace OrderAPI.Repository
{
    public interface IDeliveryTypeRepository : IManagable<DeliveryType>
    {
        Task<PaginatedResult<DeliveryType>> GetAllPaginatedAsync(int pageNumber, int pageSize);
        Task DeleteAsync(Guid id);
    }
}
