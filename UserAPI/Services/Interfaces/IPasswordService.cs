namespace UserAPI.Services.Interfaces
{
    public interface IPasswordService
    {
        Task<bool> UpdateAsync(UserDto user, string oldPassword, string newPassword);
    }
}
