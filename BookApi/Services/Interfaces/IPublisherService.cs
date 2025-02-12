using Library.Extensions;

namespace BookAPI.Services.Interfaces
{
    public interface IPublisherService
    {
        Task<PaginatedResult<PublisherDto>> GetPublishersAsync(int pageNumber, int pageSize);
        Task<PublisherDto> GetPublisherByIdAsync(Guid id);
        Task<PublisherDto> CreatePublisherAsync(PublisherDto PublisherDto);
        Task<PublisherDto> UpdatePublisherAsync(Guid id, PublisherDto PublisherDto);
        Task<bool> DeletePublisherAsync(Guid id);
    }

}