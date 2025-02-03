using AutoMapper;
using BookApi.Data;
using BookApi.Models;
using BookAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Services
{

    public class PublisherService : IPublisherService
    {
        private readonly IMapper _mapper;
        private readonly IPublisherRepository _publisherRepository;
        private readonly ILogger<PublisherService> _logger;

        public PublisherService(IMapper mapper, IPublisherRepository publisherRepository, ILogger<PublisherService> logger)
        {
            _mapper = mapper;
            _publisherRepository = publisherRepository;
            _logger = logger;
        }
        public async Task<PublisherDto> CreatePublisherAsync(PublisherDto publisherDto)
        {
            var publisher = _mapper.Map<Publisher>(publisherDto);

            await _publisherRepository.CreateAsync(publisher);

            return _mapper.Map<PublisherDto>(publisher);
        }

        public async Task<bool> DeletePublisherAsync(Guid id)
        {
            var publisher = await _publisherRepository.GetByIdAsync(id);
            if (publisher is null) return false;

            await _publisherRepository.DeleteAsync(id);

            return true;

        }

        public async Task<IEnumerable<PublisherDto>> GetPublishersAsync()
        {
            var publishers = await _publisherRepository.GetAllAsync();
            if (publishers == null || !publishers.Any())
            {
                return Enumerable.Empty<PublisherDto>();
            }
            return _mapper.Map<IEnumerable<PublisherDto>>(publishers);
        }

        public async Task<PublisherDto> GetPublisherByIdAsync(Guid id)
        {
            var publisher = await _publisherRepository.GetByIdAsync(id);

            if (publisher == null)
            {
                return null;
            }

            return _mapper.Map<PublisherDto>(publisher);
        }

        public async Task<PublisherDto> UpdatePublisherAsync(Guid id, PublisherDto publisherDto)
        {
            var existingPublisher = await _publisherRepository.GetByIdAsync(id);

            if (existingPublisher == null)
            {
                return null;
            }

            _mapper.Map(publisherDto, existingPublisher);
            await _publisherRepository.UpdateAsync(existingPublisher);

            return _mapper.Map<PublisherDto>(publisherDto);
        }
    }

}