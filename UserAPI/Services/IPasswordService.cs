namespace UserAPI.Services
{
    public interface IPasswordService
    {
        Task<PasswordDto?> GetEntityByIdAsync(Guid id);
        Task<bool> UpdateEntityAsync(UserDto user, string newPassword);
        Task<bool> DeleteEntityAsync(Guid id);
        Task<bool> AddEntityAsync(string password, UserDto userDto);
    }
}
