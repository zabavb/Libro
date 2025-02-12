using AutoMapper;
using BookApi.Data;
using BookApi.Models;
using BookAPI.Repositories.Interfaces;
using Library.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Services
{

    public class PublisherService : IPublisherService
    {
        private readonly IMapper _mapper;
        private readonly IPublisherRepository _publisherRepository;

        public PublisherService(IMapper mapper, IPublisherRepository publisherRepository)
        {
            _mapper = mapper;
            _publisherRepository = publisherRepository;
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

        public async Task<PaginatedResult<PublisherDto>> GetPublishersAsync(int pageNumber, int pageSize)
        {
            var publishers = await _publisherRepository.GetAllAsync(pageNumber, pageSize);
            if (publishers == null || publishers.Items == null)
            {
                throw new InvalidOperationException("Failed to fetch publishers.");
            }

            return new PaginatedResult<PublisherDto>
            {
                Items = _mapper.Map<ICollection<PublisherDto>>(publishers.Items),
                TotalCount = publishers.TotalCount,
                PageNumber = publishers.PageNumber,
                PageSize = pageSize
            };
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

            var pub = _mapper.Map<PublisherDto>(existingPublisher);

            return pub;
        }

    }

}