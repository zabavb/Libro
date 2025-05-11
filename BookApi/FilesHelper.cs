using Library.Common;
using SixLabors.ImageSharp;

namespace BookAPI
{
    public class FilesHelper
    {
        private readonly S3StorageService _storageService;
        private readonly string _bucketName;

        public FilesHelper(S3StorageService storageService, string bucketName)
        {
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
            _bucketName = bucketName ?? throw new ArgumentNullException(nameof(bucketName));
        }

        public IFormFile GetFormFileFromPath(string filePath, string contentType)
        {
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return new FormFile(fileStream, 0, fileStream.Length, null, Path.GetFileName(filePath))
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }

        
        public async Task<string> UploadFileFromFormAsync(
            IFormFile file,
            Guid entityId,
            string directoryPath,
            string contentType, Size? imageSize = null)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    Console.WriteLine("Файл не знайдено або він порожній.");
                    return null;
                }

                var fileName = $"{entityId}_{Path.GetFileName(file.FileName)}";

                var formFile = new FormFile(
                    file.OpenReadStream(),
                    0,
                    file.Length,
                    "file",
                    fileName)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = contentType
                };


                return await _storageService.UploadAsync(
                    _bucketName,
                    directoryPath,
                    entityId,
                    formFile,
                    imageSize);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка під час завантаження файлу: {ex.Message}");
                return null;
            }
        }

        public async Task<string> UploadPdfFromFormAsync(IFormFile pdfFile, Guid entityId, string folder)
        {
            const string contentType = "application/pdf";
            var filepath = await UploadFileFromFormAsync(pdfFile, entityId, folder, contentType);

            var uri = new Uri(filepath);
            var fileKey = uri.AbsolutePath.TrimStart('/');
            if (!string.IsNullOrEmpty(fileKey))
            {
                return _storageService.GenerateSignedUrl(_bucketName, fileKey);
            }
            return null;
        }

        public async Task<string> UploadAudioFromFormAsync(IFormFile audioFile, Guid entityId, string folder)
        {
            const string contentType = "audio/mpeg";
            var filepath = await UploadFileFromFormAsync(audioFile, entityId, folder, contentType);

            var uri = new Uri(filepath);
            var fileKey = uri.AbsolutePath.TrimStart('/');
            if (!string.IsNullOrEmpty(fileKey))
            {
                return _storageService.GenerateSignedUrl(_bucketName, fileKey);
            }
            return null;
        }

        public async Task<string> UploadImageFromFormAsync(IFormFile imageFile, Guid entityId, string folder)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                {
                    Console.WriteLine("Файл не знайдено або він порожній.");
                    return null;
                }

                var fileName = $"{entityId}{Path.GetExtension(imageFile.FileName)}";

                var formFile = new FormFile(
                    imageFile.OpenReadStream(),
                    0,
                    imageFile.Length,
                    "file",
                    fileName)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = imageFile.ContentType
                };

                var s3Url = await _storageService.UploadAsync(
                    _bucketName,
                    folder,
                    entityId,
                    formFile,
                    new Size(400, 600));

                return s3Url;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading image: {ex.Message}");
                return null;
            }
        }


    }
}
