using AutoMapper;
using BookAPI.Models;
using BookAPI.Repositories.Interfaces;
using BookAPI.Services.Interfaces;

namespace BookAPI.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<DiscountService> _logger;
        public DiscountService(IDiscountRepository repository, IMapper mapper, ILogger<DiscountService> logger) {
            _repository= repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> AddAsync(DiscountDTO discountDto)
        {
            try
            {
                _logger.LogInformation("Adding discount");
                var discount = _mapper.Map<Discount>(discountDto);
                await _repository.AddAsync(discount);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding discount");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid discountId)
        {
            try
            {
                _logger.LogInformation("Deleting discount with ID {DiscountId}", discountId);
                await _repository.DeleteAsync(discountId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting discount with ID {DiscountId}", discountId);
                return false;
            }
        }

        public async Task<DiscountDTO?> GetByIdAsync(Guid discountId)
        {
            try
            {
                _logger.LogInformation("Fetching discount with ID {DiscountId}", discountId);
                var discount = await _repository.GetByIdAsync(discountId);
                return _mapper.Map<DiscountDTO>(discount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching discount with ID {DiscountId}", discountId);
                return null;
            }
        }

        public async Task<List<DiscountDTO>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all discounts");
                var discounts = await _repository.GetAllAsync();
                return _mapper.Map<List<DiscountDTO>>(discounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all discounts");
                return new List<DiscountDTO>();
            }
        }

        public async Task<bool> UpdateAsync(DiscountDTO discountDto)
        {
            try
            {
                _logger.LogInformation("Updating discount with ID {DiscountId}", discountDto.DiscountId);
                var discount = _mapper.Map<Discount>(discountDto);
                await _repository.UpdateAsync(discount);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating discount with ID {DiscountId}", discountDto.DiscountId);
                return false;
            }
        }

    }
}
