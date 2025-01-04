using AutoMapper;
using BookApi.Data;
using BookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookApi.Services
{

    public class PublisherService : IPublisherService
    {
        private readonly BookDbContext _context;
        private readonly IMapper _mapper;

        public PublisherService(BookDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<PublisherDto> CreatePublisherAsync(PublisherDto publisherDto)
        {
            var publisher = _mapper.Map<Publisher>(publisherDto);
            publisher.Id = Guid.NewGuid();

            _context.Publishers.Add(publisher);
            await _context.SaveChangesAsync();

            return _mapper.Map<PublisherDto>(publisher);
        }

        public async Task<bool> DeletePublisherAsync(Guid id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            if (publisher is null) return false;

            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<PublisherDto>> GetPublishersAsync()
        {
            var Publishers = await _context.Publishers.ToListAsync();
            return _mapper.Map<IEnumerable<PublisherDto>>(Publishers);
        }

        public async Task<PublisherDto> GetPublisherByIdAsync(Guid id)
        {
            var publisher = await _context.Publishers.FindAsync(id);
            return publisher is not null ? _mapper.Map<PublisherDto>(publisher) : null;
        }

        public async Task<PublisherDto> UpdatePublisherAsync(Guid id, PublisherDto publisherDto)
        {
            var existingPublisher = await _context.Publishers.FirstOrDefaultAsync(a => a.Id == id);

            if (existingPublisher == null)
            {
                return null;
            }

            existingPublisher.Name = publisherDto.Name;
            existingPublisher.Description = publisherDto.Description;

            await _context.SaveChangesAsync();

            return _mapper.Map<PublisherDto>(existingPublisher);
        }
    }

}