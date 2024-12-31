using Library.Extensions;
using Library.Interfaces;
using OrderApi.Models;

namespace OrderApi.Repository.IRepository
{
    public interface IDeliveryTypeRepository : IManagable<DeliveryType>
    {
        Task<PaginatedResult<DeliveryType>> GetAllPaginatedAsync(int pageNumber, int pageSize);
    }
}
