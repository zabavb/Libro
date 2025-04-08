namespace UserAPI.Services.Interfaces
{
    public interface IPasswordService
    {
        Task<bool> UpdateAsync(Dto user, string oldPassword, string newPassword);
    }
}
