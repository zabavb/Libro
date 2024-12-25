using AutoMapper;
using Library.Extensions;
using OrderApi.Models;
using OrderApi.Repository.IRepository;

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

        public async Task<PaginatedResult<DeliveryTypeDto>> GetDeliveryTypesAsync(int pageNumber, int pageSize)
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

        public async Task<DeliveryTypeDto> GetDeliveryTypeByIdAsync(Guid id)
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

        public async Task<DeliveryTypeDto> CreateDeliveryTypeAsync(DeliveryTypeDto deliveryTypeDto)
        {
            if (deliveryTypeDto == null)
            {
                _message = "Delivery type wasn't provided.";
                _logger.LogError(_message);
                throw new ArgumentNullException(_message, nameof(deliveryTypeDto));
            }
            var deliveryType = _mapper.Map<DeliveryType>(deliveryTypeDto);

            try
            {
                await _repository.AddAsync(deliveryType);
                _logger.LogInformation("Delivery type created successfully.");
            }
            catch (Exception ex)
            {
                _message = "Error occured while adding an delivery type.";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }

            return _mapper.Map<DeliveryTypeDto>(deliveryType);
        }

        public async Task<DeliveryTypeDto> UpdateDeliveryTypeAsync(DeliveryTypeDto deliveryTypeDto)
        {
            if (deliveryTypeDto == null)
            {
                _message = "Delivery type was not provided for the update";
                _logger.LogError(_message);
                throw new ArgumentNullException(_message, nameof(deliveryTypeDto));
            }

            var deliveryType = _mapper.Map<DeliveryType>(deliveryTypeDto);
            try
            {
                await _repository.UpdateAsync(deliveryType);
                _logger.LogInformation("Delivery type updated succesfully.");
            }
            catch (InvalidOperationException)
            {
                _message = $"Delivery type with Id {deliveryTypeDto.Id} not found for update.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }
            catch (Exception ex)
            {
                _message = $"Error occured while updating the delivery type with Id [{deliveryTypeDto.Id}]";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }

            return _mapper.Map<DeliveryTypeDto>(deliveryType);
        }

        public async Task<bool> DeleteDeliveryTypeAsync(Guid id)
        {
            var deliveryType = await _repository.GetByIdAsync(id);

            if (deliveryType == null)
            {
                _message = $"Delivery type with Id [{id}] not found.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }

            try
            {
                await _repository.DeleteAsync(deliveryType);
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
            return true;
        }
    }
}
