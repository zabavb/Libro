using Microsoft.AspNetCore.Http;

namespace Library.Interfaces
{
    public interface IS3StorageService
    {
        Task<string> UploadAsync(IFormFile file, Guid id);
        Task DeleteAsync(string fileUrl);
    }
}
