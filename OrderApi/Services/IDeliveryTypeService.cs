using Library.Common;
using Library.Interfaces;
using OrderAPI;

namespace OrderApi.Services
{
    public interface IDeliveryTypeService : IManageable<DeliveryTypeDto,DeliveryTypeDto>
    {
        Task<PaginatedResult<DeliveryTypeDto>> GetAllAsync(int pageNumber, int pageSize, string searchTerm, DeliverySort? sort);
        Task DeleteAsync(Guid id);

        Task<SingleSnippet<DeliveryCardSnippet>> GetSnippetByIdAsync(Guid id);
    }
}
