
using AutoMapper;
using NuGet.Protocol.Core.Types;
using UserAPI.Models;
using UserAPI.Repositories;

namespace UserAPI.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly IPasswordRepository _repositoryPassword;
        private readonly IMapper _mapper;
        public PasswordService(IPasswordRepository repository, IMapper mapper)
        {
            _repositoryPassword = repository;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(string password, UserDto userDto)
        {
            try {
                User user = _mapper.Map<User>(userDto);
                await _repositoryPassword.AddAsync(password, user);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                // log exception
            }
            
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                await _repositoryPassword.DeleteAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                // log exception
                return false;
            }
        }

        public async Task<PasswordDto?> GetByIdAsync(Guid id)
        {
            try
            {
                var password = await _repositoryPassword.GetByIdAsync(id);
                return _mapper.Map<PasswordDto>(password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateAsync(UserDto user, string newPassword)
        {
            try
            {
                _repositoryPassword.UpdateAsync(user.Id, newPassword);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
