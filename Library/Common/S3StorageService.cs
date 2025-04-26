using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Library.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

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

        /*private async Task<IFormFile> ResizeImage(
            Stream inputStream,
            string name,
            string fileName,
            IHeaderDictionary headers,
            Size size)
        {
            using var imageResult = await Image.LoadAsync(inputStream);

            imageResult.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = size
            }));

            var outputStream = new MemoryStream();
            await imageResult.SaveAsPngAsync(outputStream);
            outputStream.Position = 0;

            var resizedFormFile = new FormFile(outputStream, 0, outputStream.Length, name, fileName)
            {
                Headers = headers,
                ContentType = "image/png"
            };

            return resizedFormFile;
        }

        public async Task<string> UploadAsync(string bucketName, string folder, Guid id, IFormFile file, Size? size)
        {
            string fileKey = $"{folder}{id}{Path.GetExtension(file.FileName)}";
            using var fs = file.OpenReadStream();

            file = await ResizeImage(fs, file.Name, file.FileName, file.Headers, size ?? new Size(500, 500));

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
        }*/

        private async Task<Stream> ResizeImage(Stream inputStream, Size size)
        {
            using var image = await Image.LoadAsync(inputStream);

            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = size
            }));

            var outputStream = new MemoryStream();
            await image.SaveAsPngAsync(outputStream);
            outputStream.Position = 0;

            return outputStream;
        }

        public async Task<string> UploadAsync(string bucketName, string folder, Guid id, IFormFile file, Size? size)
        {
            string fileKey = $"{folder}{id}{Path.GetExtension(file.FileName)}";
            Stream fs;

            if (size.HasValue)
            {
                using var inputStream = file.OpenReadStream();
                fs = await ResizeImage(inputStream, size.Value);
            }
            else
                fs = file.OpenReadStream();

            try
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = fs,
                    Key = fileKey,
                    BucketName = bucketName,
                    ContentType = file.ContentType, // "image/png"
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

        public async Task DeleteAsync(string bucketName, string fileKey)
        {
            // Removes first half of path, leaving only the directory path
            if (IsFullUrl(fileKey))
                fileKey = new Uri(fileKey).AbsolutePath.TrimStart('/');
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

        public bool IsFullUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
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