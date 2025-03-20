using AutoMapper;
using Microsoft.Extensions.Logging;
using UserAPI.Models;
using UserAPI.Repositories;
using UserAPI.Services.Interfaces;

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

        public async Task<bool> UpdateAsync(UserDto user, string oldPassword, string newPassword)
        {
            try
            {
                _logger.LogInformation("Updating password for user {UserId}", user.Id);
                await _repositoryPassword.UpdateAsync(user.Id, oldPassword, newPassword);
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
