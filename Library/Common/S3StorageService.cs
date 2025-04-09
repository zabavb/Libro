using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Library.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Library.Common
{
    public class S3StorageService(IConfiguration configuration, ILogger<IS3StorageService> logger) : IS3StorageService
    {
        private readonly string _region = configuration["AWS:Region"]!;
        private readonly IAmazonS3 _s3Client = new AmazonS3Client(
            configuration["AWS:AccessKey"],
            configuration["AWS:SecretKey"],
            Amazon.RegionEndpoint.GetBySystemName(configuration["AWS:Region"])
        );
        private readonly ILogger<IS3StorageService> _logger = logger;

        public async Task<string> UploadAsync(string bucketName, IFormFile file, string folder, Guid id)
        {
            string fileKey = $"{folder}{id}{Path.GetExtension(file.FileName)}";
            using var fs = file.OpenReadStream();
            
            try
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = fs,
                    Key = fileKey,
                    BucketName = bucketName,
                    ContentType = file.ContentType,
                    // Set the ACL based on the folder
                    CannedACL = folder.StartsWith("book/audios") || folder.StartsWith("book/e-books")
                        ? S3CannedACL.Private
                        // Temporary change
                        //: S3CannedACL.PublicRead
                        : S3CannedACL.Private
                };

                var transferUtility = new TransferUtility(_s3Client);
                await transferUtility.UploadAsync(uploadRequest);

                return $"https://{bucketName}.s3.{_region}.amazonaws.com/{fileKey}";
            }
            catch (Exception ex)
            {
                string message = "An unexpected database error occurred for user's avatar uploading.";
                _logger.LogError(message);
                throw new InvalidOperationException(message, ex);
            }
        }

        public async Task DeleteAsync(string bucketName, string fullFileKey)
        {
            // Removes first half of path, leaving only the directory path
            string fileKey = new Uri(fullFileKey).AbsolutePath.TrimStart('/');
            try
            {
                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = fileKey
                };

                await _s3Client.DeleteObjectAsync(deleteRequest);
            }
            catch (AmazonS3Exception ex)
            {
                string message = $"An unexpected database error occurred while removing user's avatar from storage.";
                _logger.LogError(message);
                throw new InvalidOperationException(message, ex);
            }
        }

        public string GenerateSignedUrl(string bucketName, string fileKey, int expirationMinutes = 20)
        {
            try
            {
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = fileKey,
                    Expires = DateTime.UtcNow.AddMinutes(expirationMinutes)
                };

                string signedUrl = _s3Client.GetPreSignedURL(request);
                Console.WriteLine($"Generated Signed URL: {signedUrl}");
                return signedUrl;
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"AmazonS3Exception: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }
    }
}