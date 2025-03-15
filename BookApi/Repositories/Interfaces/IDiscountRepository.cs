using BookAPI.Models;

namespace BookAPI.Repositories.Interfaces
{
    public interface IDiscountRepository
    {
        Task<List<Discount>> GetAllAsync();
        Task<Discount> GetByIdAsync(Guid discountId);
        Task<bool> UpdateAsync(Discount newDiscount);
        Task<bool> DeleteAsync(Guid discountId);
        Task<bool> AddAsync(Discount discount);

    }
}
