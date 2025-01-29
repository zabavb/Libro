using AutoMapper;
using Microsoft.Extensions.Logging;
using UserAPI.Models;
using UserAPI.Repositories;

namespace UserAPI.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly IPasswordRepository _repositoryPassword;
        private readonly IMapper _mapper;
        private readonly ILogger<PasswordService> _logger;

        public PasswordService(IPasswordRepository repository, IMapper mapper, ILogger<PasswordService> logger)
        {
            _repositoryPassword = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> AddAsync(string password, UserDto userDto)
        {
            try
            {
                _logger.LogInformation("Adding password for user {UserId}", userDto.Id);
                User user = _mapper.Map<User>(userDto);
                await _repositoryPassword.AddAsync(password, user);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding password for user {UserId}", userDto.Id);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting password with ID {PasswordId}", id);
                await _repositoryPassword.DeleteAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting password with ID {PasswordId}", id);
                return false;
            }
        }

        public async Task<PasswordDto?> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching password with ID {PasswordId}", id);
                var password = await _repositoryPassword.GetByIdAsync(id);
                return _mapper.Map<PasswordDto>(password);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching password with ID {PasswordId}", id);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(UserDto user, string newPassword)
        {
            try
            {
                _logger.LogInformation("Updating password for user {UserId}", user.Id);
                await _repositoryPassword.UpdateAsync(user.Id, newPassword);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating password for user {UserId}", user.Id);
                return false;
            }
        }
    }
}
