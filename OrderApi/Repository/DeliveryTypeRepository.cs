using Library.Extensions;
using Library.Interfaces;
using Microsoft.EntityFrameworkCore;
using OrderApi.Data;
using OrderApi.Models;
using OrderApi.Repository.IRepository;

namespace OrderApi.Repository
{
    public class DeliveryTypeRepository : IDeliveryTypeRepository
    {
        private readonly OrderDbContext _context;
        private readonly ILogger<IDeliveryTypeRepository> _logger;
        private string _message;
        public DeliveryTypeRepository(OrderDbContext context, ILogger<IDeliveryTypeRepository> logger)
        {
            _context = context;
            _logger = logger;
            _message = string.Empty;
        }

        public async Task<PaginatedResult<DeliveryType>> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            IEnumerable<DeliveryType> deliveryTypes = await _context.DeliveryTypes.ToListAsync();

            var totalDeliveryTypes = await Task.FromResult(deliveryTypes.Count());

            deliveryTypes = await Task.FromResult(deliveryTypes.Skip((pageNumber - 1) * pageSize).Take(pageSize));

            if (deliveryTypes == null)
            {
                _message = "Failed to fetch delivery types";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message);
            }
            else
                _logger.LogInformation("Successfully fetched delivery types");

            return new PaginatedResult<DeliveryType>
            {
                Items = (ICollection<DeliveryType>)deliveryTypes,
                TotalCount = totalDeliveryTypes,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<DeliveryType?> GetByIdAsync(Guid id)
        {
            var deliveryType = await _context.DeliveryTypes.FirstOrDefaultAsync(d => d.DeliveryId == id);
            if (deliveryType == null)
            {
                _message = $"Delivery type with Id [{id}] not found.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }
            else
                _logger.LogInformation($"Delivery type with Id [{id}] found.");
            return deliveryType == null ? null : deliveryType;
        }

        public async Task AddAsync(DeliveryType deliveryType)
        {
            if (deliveryType == null)
            {
                _message = "Delivery type was not provided for creation";
                _logger.LogError(_message);
                throw new ArgumentNullException(_message, nameof(deliveryType));
            }
            try
            {
                await _context.DeliveryTypes.AddAsync(deliveryType);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Delivery type created succesfully");
            }
            catch (ArgumentNullException ex)
            {
                _message = "Delivery type entity cannot be null.";
                _logger.LogError(_message);
                throw new ArgumentException(_message, ex);
            }
            catch (Exception ex)
            {
                _message = "Error occured while adding the Delivery type to database.";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }
        }

        public async Task UpdateAsync(DeliveryType deliveryType)
        {
            if (deliveryType == null)
            {
                _message = "Delivery type was not provided for update.";
                _logger.LogError(_message);
                throw new ArgumentNullException(_message, nameof(deliveryType));
            }

            if (!await _context.DeliveryTypes.AnyAsync(d => d.DeliveryId == deliveryType.DeliveryId))
            {
                _message = $"Delivery type with Id [{deliveryType.DeliveryId}] does not exist.";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message);
            }

            _context.DeliveryTypes.Update(deliveryType);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Delivery type with Id[{deliveryType.DeliveryId}] updated succesfully.");
        }

        async public Task DeleteAsync(Guid id)
        {
            var deliveryType = await _context.DeliveryTypes.FindAsync(id);

            if (deliveryType == null)
                throw new KeyNotFoundException();

            _context.DeliveryTypes.Remove(deliveryType);
            await _context.SaveChangesAsync();
        }
    }
}
