using Library.Extensions;
using Library.Interfaces;

namespace OrderApi.Services
{
    public interface IDeliveryTypeService : IManagable<DeliveryTypeDto>
    {
        Task<PaginatedResult<DeliveryTypeDto>> GetAllAsync(int pageNumber, int pageSize);
    }
}
