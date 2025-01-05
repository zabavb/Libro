using BookApi.Data;
using BookApi.Models;
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
            entity.Id = Guid.NewGuid();
            _context.Feedbacks.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var author = await _context.Feedbacks.FirstOrDefaultAsync(a => a.Id == id);
            if (author == null)
            {
                throw new KeyNotFoundException("Author not found");
            }
            _context.Feedbacks.Remove(author);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Feedback>> GetAllAsync()
        {
            return await _context.Feedbacks.ToListAsync();
        }

        public async Task<Feedback?> GetByIdAsync(Guid id)
        {
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
