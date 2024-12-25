using Library.Extensions;
using OrderApi.Models;

namespace OrderApi.Repository.IRepository
{
    public interface IDeliveryTypeRepository
    {
        Task<PaginatedResult<DeliveryType>> GetAllPaginatedAsync(int pageNumber, int pageSize);
        Task<DeliveryType?> GetByIdAsync(Guid id);
        Task AddAsync(DeliveryType deliveryType);
        Task DeleteAsync(DeliveryType deliveryType);
        Task UpdateAsync(DeliveryType deliveryType);
    }
}
