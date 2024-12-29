namespace UserAPI.Services
{
    public interface IPasswordService
    {
        Task<PasswordDto?> GetByIdAsync(Guid id);
        Task<bool> UpdateAsync(UserDto user, string newPassword);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> AddAsync(string password, UserDto userDto);
    }
}
