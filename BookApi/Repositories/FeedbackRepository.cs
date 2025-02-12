using BookApi.Data;
using BookApi.Models;
using BookAPI.Repositories.Interfaces;
using Library.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly BookDbContext _context;

        public FeedbackRepository(BookDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Feedback entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            entity.Id = Guid.NewGuid();
            _context.Feedbacks.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            var author = await _context.Feedbacks.FirstOrDefaultAsync(a => a.Id == id) ?? throw new KeyNotFoundException("Author not found");
            _context.Feedbacks.Remove(author);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedResult<Feedback>> GetAllAsync(int pageNumber, int pageSize)
        {
            IQueryable<Feedback> feedbacks = _context.Feedbacks.AsQueryable();

            var totalFeedbacks = await feedbacks.CountAsync();
            var resultFeedbacks = await feedbacks.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResult<Feedback>
            {
                Items = resultFeedbacks,
                TotalCount = totalFeedbacks,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }


        public async Task<Feedback?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            return await _context.Feedbacks.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task UpdateAsync(Feedback entity)
        {
            var feedbackToUpdate = await _context.Feedbacks.FirstOrDefaultAsync(a => a.Id == entity.Id) ?? throw new KeyNotFoundException("Feedback not found");
            _context.Entry(feedbackToUpdate).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }
    }
}
