using BookApi.Data;
using BookApi.Models;
using BookAPI.Repositories.Interfaces;
using Library.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Repositories
{
    public class PublisherRepository : IPublisherRepository
    {
        private readonly BookDbContext _context;

        public PublisherRepository(BookDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Publisher entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            entity.Id = Guid.NewGuid();
            _context.Publishers.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            var publisher = await _context.Publishers.FirstOrDefaultAsync(a => a.Id == id) ?? throw new KeyNotFoundException("Publisher not found");
            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedResult<Publisher>> GetAllAsync(int pageNumber, int pageSize)
        {

            IQueryable<Publisher> publishers = _context.Publishers.AsQueryable();

            var totalPublishers = await publishers.CountAsync();
            var resultPublishers = await publishers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResult<Publisher>
            {
                Items = resultPublishers,
                TotalCount = totalPublishers,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }


        public async Task<Publisher?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            return await _context.Publishers.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task UpdateAsync(Publisher entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.Id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(entity.Id));


            var existingPublisher = await _context.Publishers.FirstOrDefaultAsync(a => a.Id == entity.Id) ?? throw new KeyNotFoundException("Publisher not found");
            _context.Entry(existingPublisher).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }

    }
}
