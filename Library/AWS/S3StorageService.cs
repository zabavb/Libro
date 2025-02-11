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
        private readonly IAmazonS3 _s3Client = new AmazonS3Client(
            configuration["AWS:AccessKey"],
            configuration["AWS:SecretKey"],
            Amazon.RegionEndpoint.GetBySystemName(configuration["AWS:Region"])
        );

        public async Task<string> UploadAsync(IFormFile file, Guid id)
        {
            string fileName = $"{id}{Path.GetExtension(file.FileName)}";
            using var fs = file.OpenReadStream();
            try
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = fs,
                    Key = fileName,
                    BucketName = _bucketName,
                    ContentType = file.ContentType,
                    CannedACL = S3CannedACL.Private
                };

                var transferUtility = new TransferUtility(_s3Client);
                await transferUtility.UploadAsync(uploadRequest);

                return $"https://{_bucketName}.s3.amazonaws.com/{fileName}";
            }
            catch (Exception)
            {
                throw;  
            }
        }

        public async Task DeleteAsync(string fileUrl)
        {
            try
            {
                var uri = new Uri(fileUrl);
                string key = uri.AbsolutePath.TrimStart('/');

                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = key
                };

                await _s3Client.DeleteObjectAsync(deleteRequest);
            }
            catch (AmazonS3Exception)
            {
                throw;
            }
        }
    }
}