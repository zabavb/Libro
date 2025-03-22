namespace BookAPI.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<List<DiscountDTO>> GetAllAsync();
        Task<DiscountDTO?> GetByIdAsync(Guid discountId);
        Task<DiscountDTO?> GetByBookIdAsync(Guid id);
        Task<bool> UpdateAsync(DiscountDTO newDiscount);
        Task<bool> DeleteAsync(Guid discountId);
        Task<bool> AddAsync(DiscountDTO discount);
    }
}
