using AutoMapper;
using BookAPI.Repositories.Interfaces;
using Library.AWS;
using Library.Common;
using Library.DTOs.UserRelated.User;
using UserAPI.Models;
using UserAPI.Repositories;
using UserAPI.Repositories.Interfaces;
using UserAPI.Services.Interfaces;

namespace UserAPI.Services
{
    public class UserService(
        IUserRepository repository,
        IOrderRepository orderRepository,
        IFeedbackRepository feedbackRepository,
        /*ISubscriptionRepository subscriptionRepository,*/
        IPasswordRepository passwordRepository,

        AvatarService avatarService,
        S3StorageService storageService,
        IMapper mapper,
        ILogger<IUserService> logger
        ) : IUserService
    {
        private readonly IUserRepository _repository = repository;
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IFeedbackRepository _feedbackRepository = feedbackRepository;
        // private readonly ISubscriptionRepository _subscriptionRepository = subscriptionRepository;
        private readonly IPasswordRepository _passwordRepository = passwordRepository;

        private readonly AvatarService _avatarService = avatarService;
        private readonly IMapper _mapper = mapper;
        
        private readonly S3StorageService _storageService = storageService;
        private readonly string _folder = "user/images/";
        
        private readonly ILogger<IUserService> _logger = logger;
        private string _message = string.Empty;

        public async Task<PaginatedResult<CardDto>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            Filter? filter,
            Sort? sort
        ) {
            var users = await _repository.GetAllAsync(pageNumber, pageSize, searchTerm!, filter, sort);
            if (users == null)
            {
                _message = "Failed to fetch users.";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message);
            }
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

        private async Task<ICollection<CardDto>> MergeForCardAsync(ICollection<User> users)
        {
            List<CardDto> cards = [];
            
            foreach (var user in users)
            {
                // GetCardSnippetByUserId(int Id) returns Library/DTOs/UserRelated/User/UserCardDto/OrderCardSnippet
                var orderSnippet = await _orderRepository.GetCardSnippetByUserId(user.UserId);
                _logger.LogInformation("Successfully fetched paginated users.");
                cards.Add(_mapper.Map<CardDto>((user, orderSnippet)));
            }

            return cards;
        }

        public async Task<UserDetailsDto?> GetByIdAsync(Guid id)
        {
            var user = await _repository.GetByIdAsync(id);
            
            if (user == null)
            {
                _message = $"User with ID [{id}] not found.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }
            _logger.LogInformation($"User with ID [{id}] successfully fetched.");

            // GetAllByUserId(int id) returns IEnumerable<OrderDetailsSnippet>
            var ordersSnippet = await _orderRepository.GetAllByUserId(id);
            // GetAllByUserId(int id) returns IEnumerable<FeedbackDetailsSnippet>
            var feedbacksSnippet = await _feedbackRepository.GetAllByUserId(id);
            // GetAllByUserId(int id) returns IEnumerable<SubscriptionDetailsSnippet>
            // var subscriptionSnippet = await _subscriptionRepository.GetAllByUserId(id);

            return user == null ? null : _mapper.Map<UserDetailsDto>((user, ordersSnippet, feedbacksSnippet/*, subscriptionSnippet*/));
        }

        public async Task CreateAsync(Dto dto)
        {
            if (dto == null)
            {
                _message = "User was not provided for creation.";
                _logger.LogError(_message);
                throw new ArgumentNullException(nameof(dto), _message);
            }

            Guid id = Guid.NewGuid();

            var image = await GenerateAvatarAsync(dto.LastName, dto.FirstName);
            dto.ImageUrl = await UploadAvatarAsync(image, _folder, id);
            
            var user = _mapper.Map<User>(dto);
            user.UserId = id;

            try
            {
                await _repository.CreateAsync(user);
                _logger.LogInformation("User successfully created.");
            }
            catch (Exception ex)
            {
                _message = $"Error occurred while adding a new user.";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }
        }

        public async Task UpdateAsync(Dto dto)
        {
            if (dto == null)
            {
                _message = "User was not provided for update.";
                _logger.LogError(_message);
                throw new ArgumentNullException(nameof(dto), _message);
            }

            var user = _mapper.Map<User>(dto);
            
            try
            {
                await _repository.UpdateAsync(user);
                _logger.LogInformation($"User with ID [{dto.Id}] successfully updated.");
            }
            catch (InvalidOperationException)
            {
                _message = $"User with ID [{dto.Id}] not found for update.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }
            catch (Exception ex)
            {
                _message = $"Error occurred while updating the user with ID [{dto.Id}].";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await DeleteAvatarAsync(id);

            try
            {
                await _repository.DeleteAsync(id);
                _logger.LogInformation($"User with ID [{id}] successfully deleted.");
            }
            catch (KeyNotFoundException)
            {
                _message = $"User with ID [{id}] not found for deletion.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }
            catch (Exception ex)
            {
                _message = $"Error occurred while deleting the user with ID [{id}].";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }
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
                string message = "Error occurred while generating user's image.";
                _logger.LogError(message);
                throw new InvalidOperationException(message, ex);
            }
        }

        private async Task<string> UploadAvatarAsync(IFormFile? image, string folder, Guid id)
        {
            if (image == null || image.Length == 0)
                return null!;

            try
            {
                return await _storageService.UploadAsync(image, folder, id);
            }
            catch (Exception ex)
            {
                string message = "Error occurred while uploading user's image.";
                _logger.LogError(message);
                throw new InvalidOperationException(message, ex);
            }
        }

        private async Task DeleteAvatarAsync(Guid id)
        {
            try
            {
                var user = await _repository.GetByIdAsync(id);
                if (user == null)
                {
                    _message = $"User with ID [{id}] not found for deletion.";
                    _logger.LogError(_message);
                    throw new KeyNotFoundException(_message);
                }

                await _passwordRepository.DeleteAsync(user.PasswordId);
                string fileKey = $"{_folder}{id}.png";
                await _storageService.DeleteAsync(fileKey);
            }
            catch (Exception ex)
            {
                _message = $"Error occurred while removing user's image from storage.";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }
        }
    }
}
