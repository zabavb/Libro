using AutoMapper;
using Library.Extensions;
using OrderApi.Models;
using OrderAPI.Repository;

namespace OrderApi.Services
{
    public class DeliveryTypeService : IDeliveryTypeService
    {
        private readonly IDeliveryTypeRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<IDeliveryTypeService> _logger;
        private string _message;

        public DeliveryTypeService(IDeliveryTypeRepository repository, IMapper mapper, ILogger<IDeliveryTypeService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _message = string.Empty;
        }

        public async Task<PaginatedResult<DeliveryTypeDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var paginatedDeliveryTypes = await _repository.GetAllPaginatedAsync(pageNumber,pageSize);

            if (paginatedDeliveryTypes == null || paginatedDeliveryTypes.Items == null)
            {
                _message = "Failed to fetch paginated delivery types.";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message);
            }

            _logger.LogInformation("Delivery types successfully fetched.");

            return new PaginatedResult<DeliveryTypeDto>
            {
                Items = _mapper.Map<ICollection<DeliveryTypeDto>>(paginatedDeliveryTypes.Items),
                TotalCount = paginatedDeliveryTypes.TotalCount,
                PageNumber = paginatedDeliveryTypes.PageNumber,
                PageSize = paginatedDeliveryTypes.PageSize
            };
        }

        public async Task<DeliveryTypeDto>? GetByIdAsync(Guid id)
        {
            var deliveryType = await _repository.GetByIdAsync(id);

            if (deliveryType == null)
            {
                _message = $"Delivery type with Id [{id}] not found.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }

            _logger.LogInformation($"Delivery type with Id [{id}] fetched succesfully.");

            return deliveryType == null ? null : _mapper.Map<DeliveryTypeDto>(deliveryType);
        }

        public async Task CreateAsync(DeliveryTypeDto entity)
        {
            if (entity == null)
            {
                _message = "Delivery type was not provided for creation.";
                _logger.LogError(_message);
                throw new ArgumentNullException(null, _message);
            }
            var deliveryType = _mapper.Map<DeliveryType>(entity);

            try
            {
                deliveryType.DeliveryId = Guid.NewGuid();
                await _repository.CreateAsync(deliveryType);
                _logger.LogInformation("Delivery type created successfully.");
            }
            catch (Exception ex)
            {
                _message = "Error occured while adding an delivery type.";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }
        }

        public async Task UpdateAsync(DeliveryTypeDto entity)
        {
            if (entity == null)
            {
                _message = "Delivery type was not provided for the update";
                _logger.LogError(_message);
                throw new ArgumentNullException(null, _message);
            }

            var deliveryType = _mapper.Map<DeliveryType>(entity);
            try
            {
                await _repository.UpdateAsync(deliveryType);
                _logger.LogInformation($"Delivery type with ID [{entity.Id}]updated succesfully.");
            }
            catch (InvalidOperationException)
            {
                _message = $"Delivery type with Id {entity.Id} not found for update.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }
            catch (Exception ex)
            {
                _message = $"Error occured while updating the delivery type with Id [{entity.Id}]";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                _logger.LogInformation($"Delivery type with Id [{id}] deleted succesfully");
            }
            catch (InvalidOperationException)
            {
                _message = $"Delivery type with Id [{id}] not found for deletion.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }
            catch (Exception ex)
            {
                _message = $"Error occurred while deleting the delivery type with Id [{id}].";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }
        }
    }
}
