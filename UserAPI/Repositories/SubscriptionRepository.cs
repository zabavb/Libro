using Library.Extensions;
using Microsoft.EntityFrameworkCore;
using UserAPI.Data;
using UserAPI.Models;

namespace UserAPI.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly UserDbContext _context;

        public SubscriptionRepository(UserDbContext context) => _context = context;

        public async Task<PaginatedResult<Subscription>> GetAllAsync(int pageNumber, int pageSize, string searchTerm)
        {
            IEnumerable<Subscription> subscriptions;
            if (string.IsNullOrWhiteSpace(searchTerm))
                subscriptions = await SearchAsync(searchTerm);
            else
                subscriptions = _context.Subscriptions.AsNoTracking();

            var totalSubscriptions = await Task.FromResult(subscriptions.Count());

            subscriptions = await Task.FromResult(subscriptions.Skip((pageNumber - 1) * pageSize).Take(pageSize));
            ICollection<Subscription> result = new List<Subscription>(subscriptions);
            return new PaginatedResult<Subscription>
            {
                Items = result,
                TotalCount = totalSubscriptions,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<Subscription?> GetByIdAsync(Guid id) =>
            await _context.Subscriptions.AsNoTracking().FirstOrDefaultAsync(s => s.SubscriptionId == id);

        public async Task<IEnumerable<Subscription>> SearchAsync(string searchTerm)
        {
            var subscriptions = await _context.Subscriptions
                .AsNoTracking()
                .Where(s => s.Title.Contains(searchTerm) || s.Description!.Contains(searchTerm))
                .ToListAsync();

            return subscriptions;
        }

        public async Task CreateAsync(Subscription entity)
        {
            await _context.Subscriptions.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Subscription entity)
        {
            if (!await _context.Subscriptions.AnyAsync(s => s.SubscriptionId == entity.SubscriptionId))
                throw new InvalidOperationException();

            _context.Subscriptions.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);

            if (subscription == null)
                throw new KeyNotFoundException();

            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();
        }
    }
}
