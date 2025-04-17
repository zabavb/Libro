using BookAPI.Models;

namespace BookAPI.Repositories.Interfaces
{
    public interface IDiscountRepository
    {
        // Change from List to ICollection
        Task<List<Discount>> GetAllAsync();
        Task<Discount?> GetByIdAsync(Guid discountId);
        Task<Discount?> GetByBookIdAsync(Guid id);
        Task /*<bool>*/ UpdateAsync(Discount newDiscount);
        Task /*<bool>*/ DeleteAsync(Guid discountId);
        Task /*<bool>*/ CreateAsync(Discount discount);
    }
}