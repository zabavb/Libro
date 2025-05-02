namespace UserAPI.Services.Interfaces
{
    public interface IAvatarService
    {
        Task<IFormFile?> GenerateAvatarAsync(string firstName, string? lastName);
    }
}