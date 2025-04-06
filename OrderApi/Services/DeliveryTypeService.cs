using AutoMapper;
using Library.Common;
using Library.Interfaces;
using OrderApi.Models;
using OrderAPI;
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

        public async Task<PaginatedResult<DeliveryTypeDto>> GetAllAsync(int pageNumber, int pageSize, string searchTerm, DeliverySort? sort)
        {
            var paginatedDeliveryTypes = await _repository.GetAllPaginatedAsync(pageNumber,pageSize,searchTerm, sort);

            if (paginatedDeliveryTypes == null || paginatedDeliveryTypes.Items == null)
            {
                _message = "Failed to fetch paginated delivery types.";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message);
            }

            _logger.LogInformation("Successfully fetched [{Count}] delivery types.", paginatedDeliveryTypes.Items.Count);

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
                _message = $"Delivery type with ID [{id}] not found.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }

            _logger.LogInformation($"Delivery type with ID [{id}] successfully fetched.");

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
                _logger.LogInformation("Delivery type successfully created.");
            }
            catch (Exception ex)
            {
                _message = $"Error occurred while adding the delivery type with ID [{entity.Id}].";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }
        }

        public async Task UpdateAsync(DeliveryTypeDto entity)
        {
            if (entity == null)
            {
                _message = "Delivery type was not provided for the update.";
                _logger.LogError(_message);
                throw new ArgumentNullException(null, _message);
            }

            var deliveryType = _mapper.Map<DeliveryType>(entity);
            try
            {
                await _repository.UpdateAsync(deliveryType);
                _logger.LogInformation($"Delivery type with ID [{entity.Id}] successfully updated.");
            }
            catch (InvalidOperationException)
            {
                _message = $"Delivery type with ID [{entity.Id}] not found for update.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }
            catch (Exception ex)
            {
                _message = $"Error occured while updating the delivery type with ID [{entity.Id}].";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                _logger.LogInformation($"Delivery type with ID [{id}] successfully deleted.");
            }
            catch (KeyNotFoundException)
            {
                _message = $"Delivery type with ID [{id}] not found for deletion.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }
            catch (Exception ex)
            {
                _message = $"Error occurred while deleting the delivery type with ID [{id}].";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }
        }

        public async Task<SingleSnippet<DeliveryCardSnippet>> GetSnippetByIdAsync(Guid id)
        {
            try
            {
                var delivery = await GetByIdAsync(id);

                var snippet = new DeliveryCardSnippet()
                {
                    DeliveryId = delivery.Id,
                    ServiceName = delivery.ServiceName,
                };

                return new SingleSnippet<DeliveryCardSnippet>(false, snippet);
            }
            catch 
            {
                return new SingleSnippet<DeliveryCardSnippet>(true, new DeliveryCardSnippet());
            }
        }
    }
}
