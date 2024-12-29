
using AutoMapper;
using NuGet.Protocol.Core.Types;
using UserAPI.Models;
using UserAPI.Repositories;

namespace UserAPI.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly IPasswordRepository _repository;
        private readonly IMapper _mapper;
        public PasswordService(IPasswordRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> AddEntityAsync(string password, UserDto userDto)
        {
            try {
                User user = _mapper.Map<User>(userDto);
                await _repository.AddPasswordAsync(password, user);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                // log exception
            }
            
        }

        public async Task<bool> DeleteEntityAsync(Guid id)
        {
            try
            {
                await _repository.DeletePasswordAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                // log exception
                return false;
            }
        }

        public async Task<PasswordDto?> GetEntityByIdAsync(Guid id)
        {
            try
            {
                var password = await _repository.GetEntityByPasswordIdAsync(id);


                return _mapper.Map<PasswordDto>(password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateEntityAsync(UserDto user, string newPassword)
        {
            try
            {
                _repository.UpdatePasswordAsync(user.Id, newPassword);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
