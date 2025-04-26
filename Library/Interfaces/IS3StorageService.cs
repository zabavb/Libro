using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;

namespace Library.Interfaces
{
    public interface IS3StorageService
    {
        Task<string> UploadAsync(string bucketName, string folder, Guid id, IFormFile file, Size? size);
        Task DeleteAsync(string bucketName, string fileKey);
        string GenerateSignedUrl(string bucketName, string fileKey, int expirationMinutes = 20);
    }
}