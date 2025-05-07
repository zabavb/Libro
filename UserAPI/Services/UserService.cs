using AutoMapper;
using BookAPI.Services.Interfaces;
using Library.Common;
using Library.DTOs.UserRelated.User;
using Library.Interfaces;
using OrderAPI.Services.Interfaces;
using UserAPI.Models;
using UserAPI.Repositories;
using UserAPI.Repositories.Interfaces;
using UserAPI.Services.Interfaces;

namespace UserAPI.Services
{
    public class UserService(
        IUserRepository repository,
        ISubscriptionRepository subscriptionRepository,
        IPasswordRepository passwordRepository,
        IOrderService orderService,
        IFeedbackService feedbackService,
        AvatarService avatarService,
        IS3StorageService storageService,
        IMapper mapper,
        ILogger<IUserService> logger
    ) : IUserService
    {
        private readonly IUserRepository _repository = repository;
        private readonly ISubscriptionRepository _subscriptionRepository = subscriptionRepository;
        private readonly IPasswordRepository _passwordRepository = passwordRepository;

        private readonly IOrderService _orderService = orderService;
        private readonly IFeedbackService _feedbackService = feedbackService;

        private readonly AvatarService _avatarService = avatarService;
        private readonly IS3StorageService _storageService = storageService;
        private const string Folder = "user/images/";

        private readonly IMapper _mapper = mapper;
        private readonly ILogger<IUserService> _logger = logger;

        public async Task<PaginatedResult<CardDto>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            Filter? filter,
            Sort? sort
        )
        {
            var users = await _repository.GetAllAsync(pageNumber, pageSize, searchTerm, filter, sort);

            if (users.Items.Any())
                throw new KeyNotFoundException("No users found.");
            _logger.LogInformation("Successfully fetched paginated users.");

            var cards = await MergeForCardAsync(users.Items);

            return new PaginatedResult<CardDto>
            {
                Items = cards,
                TotalCount = users.TotalCount,
                PageNumber = users.PageNumber,
                PageSize = users.PageSize
            };
        }

        public async Task<UserDetailsDto?> GetByIdAsync(Guid id)
        {
            var user = await _repository.GetByIdAsync(id) ??
                       throw new KeyNotFoundException($"User with ID [{id}] not found.");
            _logger.LogInformation("User with ID [{id}] successfully fetched.", id);

            return await MergeForDetailsAsync(id, user);
        }

        public async Task CreateAsync(Dto dto)
        {
            if (dto == null)
                throw new ArgumentException("User data mast be provided.", nameof(dto));

            var id = Guid.NewGuid();

            var image = await GenerateAvatarAsync(dto.LastName, dto.FirstName);
            dto.ImageUrl = await UploadAvatarAsync(image, Folder, id);

            var user = _mapper.Map<User>(dto);
            user.UserId = id;
            await _repository.CreateAsync(user);
            _logger.LogInformation("User successfully created.");
        }

        public async Task UpdateAsync(Dto dto)
        {
            if (dto == null)
                throw new ArgumentException("User data mast be provided.", nameof(dto));

            var user = _mapper.Map<User>(dto);
            await _repository.UpdateAsync(user);
            _logger.LogInformation("User with ID [{id}] successfully updated.", dto.Id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await DeleteAvatarAsync(id);
            await _passwordRepository.DeleteAsync(user.PasswordId);
            await _repository.DeleteAsync(id);
            _logger.LogInformation($"User with ID [{id}] successfully deleted.");
        }

        private async Task<ICollection<CardDto>> MergeForCardAsync(ICollection<User> users)
        {
            var tasks = users.Select(async user =>
            {
                var orderSnippet = await _orderService.GetCardSnippetByUserIdAsync(user.UserId);
                return _mapper.Map<CardDto>((user, orderSnippet));
            });

            return await Task.WhenAll(tasks);
        }

        private async Task<UserDetailsDto> MergeForDetailsAsync(Guid id, User user)
        {
            var ordersSnippetTask = _orderService.GetAllByUserIdAsync(id);
            var feedbacksSnippetTask = _feedbackService.GetAllByUserIdAsync(id);

            await Task.WhenAll(ordersSnippetTask, feedbacksSnippetTask);

            var ordersSnippet = await ordersSnippetTask;
            var feedbacksSnippet = await feedbacksSnippetTask;

            // If both failed, return only user data
            if (ordersSnippet.IsFailedToFetch && feedbacksSnippet.IsFailedToFetch)
                return _mapper.Map<UserDetailsDto>(user);

            // If only orders failed, return user + feedbacks
            if (ordersSnippet.IsFailedToFetch)
                return _mapper.Map<UserDetailsDto>((user, feedbacksSnippet));

            // If only feedbacks failed, return user + orders
            if (feedbacksSnippet.IsFailedToFetch)
                return _mapper.Map<UserDetailsDto>((user, ordersSnippet));

            // If both succeeded, return full mapped data
            return _mapper.Map<UserDetailsDto>((user, ordersSnippet, feedbacksSnippet));
        }

        private async Task<IFormFile?> GenerateAvatarAsync(string? lastName, string firstName)
        {
            try
            {
                byte[] avatarImage = await _avatarService.GenerateAvatarAsync(firstName, lastName);
                var stream = new MemoryStream(avatarImage);
                IFormFile formFile = new FormFile(stream, 0, avatarImage.Length, "file", "avatar.png")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/png"
                };

                return formFile;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Database error for user's avatar generation.", ex);
            }
        }

        private async Task<string> UploadAvatarAsync(IFormFile? image, string folder, Guid id) =>
            image != null && image.Length > 0
                ? await _storageService.UploadAsync(GlobalDefaults.BucketName, image, folder, id)
                : string.Empty;

        private async Task<User> DeleteAvatarAsync(Guid id)
        {
            var user = await _repository.GetByIdAsync(id) ??
                       throw new KeyNotFoundException($"User with ID [{id}] not found for deletion.");

            string fileKey = $"{Folder}{id}.png";
            await _storageService.DeleteAsync(GlobalDefaults.BucketName, fileKey);

            return user;
        }
    }
}