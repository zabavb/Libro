using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Library.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Library.AWS
{
    public class S3StorageService(IConfiguration configuration) : IS3StorageService
    {
        private readonly string _bucketName = configuration["AWS:BucketName"]!;
        private readonly string _region = configuration["AWS:Region"]!;
        private readonly IAmazonS3 _s3Client = new AmazonS3Client(
            configuration["AWS:AccessKey"],
            configuration["AWS:SecretKey"],
            Amazon.RegionEndpoint.GetBySystemName(configuration["AWS:Region"])
        );

        public async Task<string> UploadAsync(IFormFile file, string folder, Guid id)
        {
            string fileKey = $"{folder}{id}{Path.GetExtension(file.FileName)}";
            using var fs = file.OpenReadStream();
            
            try
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = fs,
                    Key = fileKey,
                    BucketName = _bucketName,
                    ContentType = file.ContentType,
                    // Set the ACL based on the folder
                    CannedACL = folder.StartsWith("book/audios") || folder.StartsWith("book/e-books")
                        ? S3CannedACL.Private
                        : S3CannedACL.PublicRead
                };

                var transferUtility = new TransferUtility(_s3Client);
                await transferUtility.UploadAsync(uploadRequest);

                return $"https://{_bucketName}.s3.{_region}.amazonaws.com/{fileKey}";
            }
            catch (Exception)
            {
                throw;  
            }
        }

        public async Task DeleteAsync(string fileKey)
        {
            try
            {
                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileKey
                };

                await _s3Client.DeleteObjectAsync(deleteRequest);
            }
            catch (AmazonS3Exception)
            {
                throw;
            }
        }

        public string GenerateSignedUrl(string fileKey, int expirationMinutes = 20)
        {
            try
            {
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = _bucketName,
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