using AutoMapper;
using UserAPI.Services.Interfaces;
using Library.Common;
using Library.DTOs.UserRelated.Subscription;
using Library.Interfaces;
using SixLabors.ImageSharp;
using UserAPI.Models.Filters;
using UserAPI.Models.Subscription;
using UserAPI.Repositories.Interfaces;

namespace UserAPI.Services
{
    public class SubscriptionService(
        ISubscriptionRepository repository,
        IS3StorageService storageService,
        IMapper mapper,
        ILogger<ISubscriptionService> logger
    ) : ISubscriptionService
    {
        private readonly ISubscriptionRepository _repository = repository;

        private readonly IS3StorageService _storageService = storageService;

        private readonly IMapper _mapper = mapper;
        private readonly ILogger<ISubscriptionService> _logger = logger;


        public async Task<PaginatedResult<SubscriptionCardDto>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm
        )
        {
            var subscriptions = await _repository.GetAllAsync(pageNumber, pageSize, searchTerm);

            if (subscriptions.Items.Count == 0)
            {
                _logger.LogInformation("No subscriptions found.");
                return new();
            }

            _logger.LogInformation("Successfully fetched paginated subscriptions.");

            var cards = subscriptions.Items
                .Select(subscription => _mapper.Map<SubscriptionCardDto>(subscription))
                .ToList();
            return new PaginatedResult<SubscriptionCardDto>
            {
                Items = cards,
                TotalCount = subscriptions.TotalCount,
                PageNumber = subscriptions.PageNumber,
                PageSize = subscriptions.PageSize
            };
        }

        public async Task<SubscriptionDto?> GetByIdAsync(Guid id)
        {
            var subscription = await _repository.GetByIdAsync(id) ??
                               throw new KeyNotFoundException($"Subscription with ID [{id}] not found.");
            _logger.LogInformation("Subscription with ID [{id}] successfully fetched.", id);

            return _mapper.Map<SubscriptionDto>(subscription);
        }

        public async Task CreateAsync(SubscriptionDto dto)
        {
            if (dto == null)
                throw new ArgumentException("Subscription data mast be provided.", nameof(dto));

            var id = Guid.NewGuid();
            if (dto.Image != null)
                dto.ImageUrl =
                    await _storageService.UploadAsync(GlobalDefaults.BucketName,
                        GlobalDefaults.SubscriptionImagesFolder, id, dto.Image, GlobalDefaults.SubscriptionImagesSize);

            var subscription = _mapper.Map<Subscription>(dto);
            subscription.SubscriptionId = id;

            await _repository.CreateAsync(subscription);
            _logger.LogInformation($"Subscription successfully created.");
        }

        public async Task UpdateAsync(SubscriptionDto dto)
        {
            if (dto == null)
                throw new ArgumentException("Subscription data mast be provided.", nameof(dto));

            var existingSubscription = await _repository.GetByIdAsync(dto.Id) ??
                                       throw new KeyNotFoundException($"Subscription with ID [{dto.Id}] not found.");

            if (dto.Image != null)
            {
                if (!string.IsNullOrEmpty(existingSubscription.ImageUrl))
                    await _storageService.DeleteAsync(GlobalDefaults.BucketName, existingSubscription.ImageUrl);

                dto.ImageUrl =
                    await _storageService.UploadAsync(GlobalDefaults.BucketName,
                        GlobalDefaults.SubscriptionImagesFolder, dto.Id, dto.Image,
                        GlobalDefaults.SubscriptionImagesSize);
            }

            var subscription = _mapper.Map<Subscription>(dto);

            await _repository.UpdateAsync(subscription);
            _logger.LogInformation("Subscription with ID [{id}] successfully updated.", dto.Id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var existingSubscription = await _repository.GetByIdAsync(id) ??
                                       throw new KeyNotFoundException($"Subscription with ID [{id}] not found.");

            if (!string.IsNullOrEmpty(existingSubscription.ImageUrl))
                await _storageService.DeleteAsync(GlobalDefaults.BucketName, existingSubscription.ImageUrl);

            await _repository.DeleteAsync(id);
            _logger.LogInformation($"Subscription with ID [{id}] successfully deleted.");
        }

        public async Task SubscribeAsync(SubscribeRequest request)
        {
            await _repository.SubscribeAsync(request.SubscriptionId, request.UserId);
            _logger.LogInformation("User with ID [{id}] successfully subscribed for ID [{id}].", request.UserId,
                request.SubscriptionId);
        }

        public async Task UnsubscribeAsync(SubscribeRequest request)
        {
            await _repository.UnsubscribeAsync(request.SubscriptionId, request.UserId);
            _logger.LogInformation("User with ID [{id}] successfully UNsubscribed from ID [{id}].", request.UserId,
                request.SubscriptionId);
        }

        public async Task<int> GetActiveSubscriptionsCountAsync()
        {
            return await _repository.GetActiveSubscriptionsCountAsync();
        }

        public async Task<ICollection<BySubscription>> GetAllForFilterContentAsync()
        {
            var subscriptions = await _repository.GetAllForFilterContentAsync();
            _logger.LogInformation("Subscriptions successfully fetched.");

            return subscriptions;
        }
    }
}