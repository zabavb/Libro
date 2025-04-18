using BookAPI.Models.Sortings;
using Library.Common;
using Library.Interfaces;

namespace BookAPI.Services.Interfaces
{
    public interface IPublisherService : IManageable<PublisherDto>
    {
        // Renamed from "GetPublishersAsync" to "GetAllAsync"
        Task<PaginatedResult<PublisherDto>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string searchTerm,
            PublisherSort? sort
        );

        /*Task<PublisherDto> GetPublisherByIdAsync(Guid id);
        Task<PublisherDto> CreatePublisherAsync(PublisherDto PublisherDto);
        Task<PublisherDto> UpdatePublisherAsync(Guid id, PublisherDto PublisherDto);
        Task<bool> DeletePublisherAsync(Guid id);*/
    }
}