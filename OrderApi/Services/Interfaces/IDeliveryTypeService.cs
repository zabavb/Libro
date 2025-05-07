using Library.Common;
using Library.Interfaces;
using OrderAPI;

namespace OrderAPI.Services.Interfaces
{
    public interface IDeliveryTypeService : IManageable<DeliveryTypeDto,DeliveryTypeDto>
    {
        Task<PaginatedResult<DeliveryTypeDto>> GetAllAsync(int pageNumber, int pageSize, string searchTerm, DeliverySort? sort);

        Task DeleteAsync(Guid id);
    }
}
