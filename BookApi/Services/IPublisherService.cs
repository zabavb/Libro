namespace BookApi.Services
{
    public interface IPublisherService
    {
        Task<IEnumerable<PublisherDto>> GetPublishersAsync();
        Task<PublisherDto> GetPublisherByIdAsync(Guid id);
        Task<PublisherDto> CreatePublisherAsync(PublisherDto PublisherDto);
        Task<PublisherDto> UpdatePublisherAsync(Guid id, PublisherDto PublisherDto);
        Task<bool> DeletePublisherAsync(Guid id);
    }

}