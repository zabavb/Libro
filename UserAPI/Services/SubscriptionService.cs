using UserAPI.Repositories;
using AutoMapper;
using UserAPI.Models;
using Library.Extensions;
using UserAPI.Services.Interfaces;

namespace UserAPI.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ISubscriptionService> _logger;
        private string _message;

        public SubscriptionService(ISubscriptionRepository repository, IMapper mapper, ILogger<ISubscriptionService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _message = string.Empty;
        }

        public async Task<PaginatedResult<SubscriptionDto>> GetAllAsync(int pageNumber, int pageSize, string searchTerm)
        {
            var paginatedSubscriptions = await _repository.GetAllAsync(pageNumber, pageSize, searchTerm);

            if (paginatedSubscriptions == null || paginatedSubscriptions.Items == null)
            {
                _message = "Failed to fetch paginated subscriptions.";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message);
            }
            _logger.LogInformation("Successfully fetched [{Count}] users.", paginatedSubscriptions.Items.Count());

            return new PaginatedResult<SubscriptionDto>
            {
                Items = _mapper.Map<ICollection<SubscriptionDto>>(paginatedSubscriptions.Items),
                TotalCount = paginatedSubscriptions.TotalCount,
                PageNumber = paginatedSubscriptions.PageNumber,
                PageSize = paginatedSubscriptions.PageSize
            };
        }

        public async Task<SubscriptionDto?> GetByIdAsync(Guid id)
        {
            var subscription = await _repository.GetByIdAsync(id);

            if (subscription == null)
            {
                _message = $"Subscription with ID [{id}] not found.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }

            _logger.LogInformation($"Subscription with ID [{id}] successfully fetched.");
            return subscription == null ? null : _mapper.Map<SubscriptionDto>(subscription);
        }

        public async Task CreateAsync(SubscriptionDto entity)
        {
            if (entity == null)
            {
                _message = "Subscription was not provided for creation.";
                _logger.LogError(_message);
                throw new ArgumentNullException(_message, nameof(entity));
            }

            var subscription = _mapper.Map<Subscription>(entity);
            try
            {
                await _repository.CreateAsync(subscription);
                _logger.LogInformation($"Subscription successfully created.");
            }
            catch (Exception ex)
            {
                _message = $"Error occurred while adding the subscription with ID [{entity.Id}].";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }
        }

        public async Task UpdateAsync(SubscriptionDto entity)
        {
            if (entity == null)
            {
                _message = "Subscription was not provided for update.";
                _logger.LogError(_message);
                throw new ArgumentNullException(_message, nameof(entity));
            }

            var subscription = _mapper.Map<Subscription>(entity);
            try
            {
                await _repository.UpdateAsync(subscription);
                _logger.LogInformation($"User with ID [{entity.Id}] successfully updated.");
            }
            catch (InvalidOperationException)
            {
                _message = $"Subscription with ID {entity.Id} not found for update.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }
            catch (Exception ex)
            {
                _message = $"Error occurred while updating the subscription with ID [{entity.Id}].";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                _logger.LogInformation($"Subscription with ID [{id}] successfully deleted.");
            }
            catch (KeyNotFoundException)
            {
                _message = $"Subscription with ID {id} not found for deletion.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }
            catch (Exception ex)
            {
                _message = $"Error occurred while deleting the subscription with ID [{id}].";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }
        }
    }
}
