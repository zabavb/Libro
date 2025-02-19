using AutoMapper;
using Library.AWS;
using Library.Extensions;
using UserAPI.Models;
using UserAPI.Repositories;

namespace UserAPI.Services
{
    public class UserService(IUserRepository repository, S3StorageService storageService, IMapper mapper, ILogger<IUserService> logger) : IUserService
    {
        private readonly IUserRepository _repository = repository;
        private readonly S3StorageService _storageService = storageService;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<IUserService> _logger = logger;
        private string _message = string.Empty;

        public async Task<PaginatedResult<UserDto>> GetAllAsync(int pageNumber, int pageSize, string? searchTerm, Filter? filter, Sort? sort)
        {
            var paginatedUsers = await _repository.GetAllAsync(pageNumber, pageSize, searchTerm!, filter, sort);

            if (paginatedUsers == null || paginatedUsers.Items == null)
            {
                _message = "Failed to fetch paginated users.";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message);
            }
            _logger.LogInformation("Successfully fetched [{Count}] users.", paginatedUsers.Items.Count);

            return new PaginatedResult<UserDto>
            {
                Items = _mapper.Map<ICollection<UserDto>>(paginatedUsers.Items),
                TotalCount = paginatedUsers.TotalCount,
                PageNumber = paginatedUsers.PageNumber,
                PageSize = paginatedUsers.PageSize
            };
        }

        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            var user = await _repository.GetByIdAsync(id);
            
            if (user == null)
            {
                _message = $"User with ID [{id}] not found.";
                _logger.LogError(_message);
                throw new KeyNotFoundException(_message);
            }
            _logger.LogInformation($"User with ID [{id}] successfully fetched.");
            
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task CreateAsync(UserDto dto)
        {
            if (dto == null)
            {
                _message = "User was not provided for creation.";
                _logger.LogError(_message);
                throw new ArgumentNullException(nameof(dto), _message);
            }

            Guid id = Guid.NewGuid();
            
            dto.ImageUrl = await UploadImageAsync(dto.Image, id);
            
            var user = _mapper.Map<User>(dto);
            user.UserId = id;

            try
            {
                await _repository.CreateAsync(user);
                _logger.LogInformation("User successfully created.");
            }
            catch (Exception ex)
            {
                _message = $"Error occurred while adding the user with ID [{dto.Id}].";
                _logger.LogError(_message);
                throw new InvalidOperationException(_message, ex);
            }
        }

        public async Task UpdateAsync(UserDto dto)
        {
            if (dto == null)
            {
                _message = "User was not provided for update.";
                _logger.LogError(_message);
                throw new ArgumentNullException(nameof(dto), _message);
            }

            if (dto.Image != null)
            {
                try
                {
                    await _storageService.DeleteAsync(dto.ImageUrl!);
                }
                catch (Exception ex)
                {
                    _message = $"Error occurred while removing user's image from storage.";
                    _logger.LogError(_message);
                    throw new InvalidOperationException(_message, ex);
                }

                dto.ImageUrl = await UploadImageAsync(dto.Image, dto.Id);
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

        public async Task DeleteAsync(Guid id, string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                try
                {
                    await _storageService.DeleteAsync(imageUrl);
                }
                catch (Exception ex)
                {
                    _message = $"Error occurred while removing user's image from storage.";
                    _logger.LogError(_message);
                    throw new InvalidOperationException(_message, ex);
                }
            }

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

        private async Task<string?> UploadImageAsync(IFormFile? image, Guid id)
        {
            if (image == null || image.Length == 0)
                return null;

            try
            {
                return await _storageService.UploadAsync(image, id);
            }
            catch (Exception ex)
            {
                string message = "Error occurred while uploading user's image.";
                _logger.LogError(message);
                throw new InvalidOperationException(message, ex);
            }
        }
    }
}
