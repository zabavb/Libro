using BookAPI.Data;
using BookAPI.Models;
using BookAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookAPI.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly BookDbContext _context;
        private readonly ILogger<IDiscountRepository> _logger;
        public DiscountRepository(BookDbContext context, ILogger<IDiscountRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Discount?> GetByIdAsync(Guid discountId)
        {
            var res = await _context.Discounts.FirstOrDefaultAsync(p => p.DiscountId == discountId);
            if (res == null) {
                _logger.LogInformation($"Discount not found by id {discountId}");
            }
            else
            {
                _logger.LogInformation($"Discount found by id {discountId}");
            }
            return res;
        }

        public async Task<Discount?> GetByBookIdAsync(Guid id)
        {
            var res = await _context.Discounts.FirstOrDefaultAsync(p => p.BookId == id);
            if (res == null)
            {
                _logger.LogInformation($"Discount not found by BookId {id}");
            }
            else
            {
                _logger.LogInformation($"Discount found by BookId {id}");
            }
            return res;
        }

        public async Task<bool> UpdateAsync(Discount newDiscount)
        {
            var discountEntity = await GetByIdAsync(newDiscount.DiscountId);
            if (discountEntity == null)
            {
                return false;
            }
            discountEntity = newDiscount;
            _context.Discounts.Update(discountEntity);
            if (await _context.SaveChangesAsync() < 1)
            {
                _logger.LogInformation("Failed to update discount rate");
                return false;
            }

            _logger.LogInformation("Discount rate updated");
            return true;
        }

        public async Task<bool> DeleteAsync(Guid discountId)
        {
            var discountEntity = await GetByIdAsync(discountId);
            if (discountEntity == null)
            {
                return false;
            }
            _context.Discounts.Remove(discountEntity);
            if (await _context.SaveChangesAsync() < 1)
            {
                _logger.LogInformation("Failed to delete discount");
                return false;
            }
            _logger.LogInformation("Discount deleted");
            return true;
        }

        public async Task<bool> AddAsync(Discount discount)
        {
            await _context.Discounts.AddAsync(discount);
            if (await _context.SaveChangesAsync() < 1)
            {
                _logger.LogInformation("Failed to add discount");
                return false;
            }
            _logger.LogInformation("Discount added");
            return true;
        }

        public async Task<List<Discount>> GetAllAsync()
        {
            return await _context.Discounts.ToListAsync();
        }

    }
}
