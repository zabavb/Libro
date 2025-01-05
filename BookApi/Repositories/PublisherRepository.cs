using BookApi.Data;
using BookApi.Models;
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
            entity.Id = Guid.NewGuid();
            _context.Publishers.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var publisher = await _context.Publishers.FirstOrDefaultAsync(a => a.Id == id);
            if (publisher == null)
            {
                throw new KeyNotFoundException("Publisher not found");
            }
            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Publisher>> GetAllAsync()
        {
            return await _context.Publishers.ToListAsync();
        }

        public async Task<Publisher?> GetByIdAsync(Guid id)
        {
            return await _context.Publishers.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task UpdateAsync(Publisher entity)
        {
            var existingPublisher = await _context.Publishers.FirstOrDefaultAsync(a => a.Id == entity.Id);
            if (existingPublisher == null)
            {
                throw new KeyNotFoundException("publisher not found");
            }

            existingPublisher.Name = entity.Name;
            existingPublisher.Description = entity.Description;

            await _context.SaveChangesAsync();
        }
    }
}
