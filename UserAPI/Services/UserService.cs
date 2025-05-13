using AutoMapper;
using Library.Common;
using Library.DTOs.UserRelated.User;
using Library.Interfaces;
using OrderAPI.Services.Interfaces;
using UserAPI.Models;
using UserAPI.Repositories.Interfaces;
using UserAPI.Services.Interfaces;

namespace UserAPI.Services
{
    public class UserService(
        IUserRepository repository,
        ISubscriptionRepository subscriptionRepository,
        IPasswordRepository passwordRepository,
        IAvatarService avatarService,
        IS3StorageService storageService,
        IMapper mapper,
        ILogger<IUserService> logger
    ) : IUserService
    {
        private readonly IUserRepository _repository = repository;
        private readonly ISubscriptionRepository _subscriptionRepository = subscriptionRepository;
        private readonly IPasswordRepository _passwordRepository = passwordRepository;

        private readonly IAvatarService _avatarService = avatarService;
        private readonly IS3StorageService _storageService = storageService;

        private readonly IMapper _mapper = mapper;
        private readonly ILogger<IUserService> _logger = logger;

        public async Task<PaginatedResult<Dto>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? searchTerm,
            Filter? filter,
            Sort? sort
        )
        {
            var users = await _repository.GetAllAsync(pageNumber, pageSize, searchTerm, filter, sort);

            _logger.LogInformation("Successfully fetched paginated users.");

            var dtos = users.Items.Select(user => _mapper.Map<Dto>(user)).ToList();
            return new PaginatedResult<Dto>
            {
                Items = dtos,
                TotalCount = users.TotalCount,
                PageNumber = users.PageNumber,
                PageSize = users.PageSize
            };
        }

        public async Task<UserWithSubscriptionsDto?> GetByIdAsync(Guid id)
        {
            var user = await _repository.GetByIdAsync(id) ??
                       throw new KeyNotFoundException($"User with ID [{id}] not found.");
            _logger.LogInformation("User with ID [{id}] successfully fetched.", id);

            return _mapper.Map<UserWithSubscriptionsDto>(user);
        }

        public async Task CreateAsync(Dto dto)
        {
            if (dto == null)
                throw new ArgumentException("User data mast be provided.", nameof(dto));

            if (dto.Id == Guid.Empty)
                dto.Id = Guid.NewGuid();

            var image = await _avatarService.GenerateAvatarAsync(dto.FirstName, dto.LastName);
            dto.ImageUrl = await UploadAvatarAsync(image, GlobalDefaults.UserImagesFolder, dto.Id);

            var user = _mapper.Map<User>(dto);
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
            await DeleteAvatarAsync(id);
            await _repository.DeleteAsync(id);
            _logger.LogInformation($"User with ID [{id}] successfully deleted.");
        }

        private async Task<string> UploadAvatarAsync(IFormFile? image, string folder, Guid id) =>
            image != null && image.Length > 0
                ? await _storageService.UploadAsync(GlobalDefaults.BucketName, folder, id, image,
                    GlobalDefaults.UserImagesSize)
                : string.Empty;

        private async Task DeleteAvatarAsync(Guid id)
        {
            _ = await _repository.GetByIdAsync(id) ??
                throw new KeyNotFoundException($"User with ID [{id}] not found for deletion.");

            string fileKey = $"{GlobalDefaults.UserImagesFolder}{id}.png";
            await _storageService.DeleteAsync(GlobalDefaults.BucketName, fileKey);
        }
    }
}