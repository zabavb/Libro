using Library.Extensions;

namespace OrderApi.Services
{
    public interface IDeliveryTypeService
    {
        Task<PaginatedResult<DeliveryTypeDto>> GetDeliveryTypesAsync(int pageNumber, int pageSize);
        Task<DeliveryTypeDto> GetDeliveryTypeByIdAsync(Guid id);
        Task<DeliveryTypeDto> CreateDeliveryTypeAsync(DeliveryTypeDto deliveryTypeDto);
        Task<DeliveryTypeDto> UpdateDeliveryTypeAsync(DeliveryTypeDto deliveryTypeDto);
        Task<bool> DeleteDeliveryTypeAsync(Guid id);
    }
}
