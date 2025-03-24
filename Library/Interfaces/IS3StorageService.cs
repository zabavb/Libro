using Microsoft.AspNetCore.Http;

namespace Library.Interfaces
{
    public interface IS3StorageService
    {
        Task<string> UploadAsync(IFormFile file, string folder, Guid id);
        Task DeleteAsync(string fileKey);
        string GenerateSignedUrl(string fileKey, int expirationMinutes = 20);
    }
}
