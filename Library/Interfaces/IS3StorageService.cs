using Microsoft.AspNetCore.Http;

namespace Library.Interfaces
{
    public interface IS3StorageService
    {
        Task<string> UploadAsync(string bucketName, IFormFile file, string folder, Guid id);
        Task DeleteAsync(string bucketName, string fileKey);
        string GenerateSignedUrl(string bucketName, string fileKey, int expirationMinutes = 20);
    }
}
