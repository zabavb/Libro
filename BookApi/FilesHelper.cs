using Library.Common;

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


        public async Task<string> UploadFileAsync(
            string localFilePath,
            Guid entityId,
            string directoryPath,
            string contentType)
        {
            try
            {
                if (!File.Exists(localFilePath))
                {
                    Console.WriteLine($"Файл не знайдено: {localFilePath}");
                    return null;
                }

                var fileName = Path.GetFileName(localFilePath);

                await using var fileStream = File.OpenRead(localFilePath);
                var formFile = new FormFile(
                    fileStream,
                    0,
                    fileStream.Length,
                    "file",
                    fileName)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = contentType
                };

                return await _storageService.UploadAsync(
                    _bucketName,
                    formFile,
                    directoryPath,
                    entityId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка під час завантаження файлу: {ex.Message}");
                return null;
            }
        }

        public static string GetAudioContentType(string fileExtension)
        {
            return fileExtension.ToLower() switch
            {
                ".mp3" => "audio/mpeg",
                ".mp4" => "audio/mp4",
                ".ogg" => "audio/ogg",
                ".wav" => "audio/wav",
                ".aac" => "audio/aac",
                ".webm" => "audio/webm",
                ".flac" => "audio/flac",
                ".m4a" => "audio/m4a",
                _ => "application/octet-stream"
            };
        }
        

        public async Task<string> UploadPdfAsync(string localFilePath, Guid entityId)
        {
            const string contentType = "application/pdf";
            var filepath = await UploadFileAsync(localFilePath, entityId, "book/e-books/", contentType);
            var uri = new Uri(filepath);
            var fileKey = uri.AbsolutePath.TrimStart('/');
            if (!string.IsNullOrEmpty(fileKey))
            {
                return _storageService.GenerateSignedUrl(_bucketName, fileKey);
            }
            return null;
        }
        public async Task<string> UploadAudioAsync(string localFilePath, Guid entityId)
        {
            const string contentType = "audio/mpeg";
            var filepath = await UploadFileAsync(localFilePath, entityId, "book/audios/", contentType);

            var uri = new Uri(filepath);
            var fileKey = uri.AbsolutePath.TrimStart('/');
            if (!string.IsNullOrEmpty(fileKey))
            {
                var a = _storageService.GenerateSignedUrl(_bucketName, fileKey);
                return a;
            }
            return null;
        }

        //public async Task<string> UploadAudioAsync(
        //    string localFilePath,
        //    Guid entityId,
        //    string bookTitle)
        //{
        //    var fileExtension = Path.GetExtension(localFilePath).ToLower();
        //    var contentType = GetAudioContentType(fileExtension);
        //    var filepath = await UploadFileAsync(localFilePath, entityId, "book/audios/", contentType);
        //    var uri = new Uri(filepath);
        //    var fileKey = uri.AbsolutePath.TrimStart('/');

        //    if (!string.IsNullOrEmpty(fileKey))
        //    {
        //        return _storageService.GenerateSignedUrl(_bucketName, fileKey);
        //    }

        //    return null;
        //}


        public async Task<string> UploadImageAsync(string imageUrl, Guid entityId)
        {
            string tempFileName = null;
            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(imageUrl);

                if (!response.IsSuccessStatusCode)
                    return null;

                tempFileName = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}{Path.GetExtension(imageUrl)}");

                await using (var fileStream = File.Create(tempFileName))
                {
                    await response.Content.CopyToAsync(fileStream);
                }

                var fileStreamForForm = new FileStream(tempFileName, FileMode.Open, FileAccess.Read);
                var formFile = new FormFile(
                    fileStreamForForm,
                    0,
                    fileStreamForForm.Length,
                    "file",
                    $"{entityId}{Path.GetExtension(imageUrl)}")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = response.Content.Headers.ContentType?.MediaType ?? "application/octet-stream"
                };

                var s3Url = await _storageService.UploadAsync(
                    _bucketName,
                    formFile,
                    "book/images/",
                    entityId);

                return s3Url;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading image: {ex.Message}");
                return null;
            }
            finally
            {
                if (tempFileName != null && File.Exists(tempFileName))
                {
                    try
                    {
                        File.Delete(tempFileName);
                    }
                    catch { }
                }
            }

           
        }
        public async Task<string> UploadFileFromFormAsync(
    IFormFile file,
    Guid entityId,
    string directoryPath,
    string contentType)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    Console.WriteLine("Файл не знайдено або він порожній.");
                    return null;
                }

                // Генеруємо унікальне ім'я для файлу
                var fileName = $"{entityId}_{Path.GetFileName(file.FileName)}";

                // Завантажуємо файл в storage
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
                    formFile,
                    directoryPath,
                    entityId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка під час завантаження файлу: {ex.Message}");
                return null;
            }
        }

        public async Task<string> UploadPdfFromFormAsync(IFormFile pdfFile, Guid entityId)
        {
            const string contentType = "application/pdf";
            var filepath = await UploadFileFromFormAsync(pdfFile, entityId, "book/e-books/", contentType);

            var uri = new Uri(filepath);
            var fileKey = uri.AbsolutePath.TrimStart('/');
            if (!string.IsNullOrEmpty(fileKey))
            {
                return _storageService.GenerateSignedUrl(_bucketName, fileKey);
            }
            return null;
        }

        public async Task<string> UploadAudioFromFormAsync(IFormFile audioFile, Guid entityId)
        {
            const string contentType = "audio/mpeg";
            var filepath = await UploadFileFromFormAsync(audioFile, entityId, "book/audios/", contentType);

            var uri = new Uri(filepath);
            var fileKey = uri.AbsolutePath.TrimStart('/');
            if (!string.IsNullOrEmpty(fileKey))
            {
                return _storageService.GenerateSignedUrl(_bucketName, fileKey);
            }
            return null;
        }

        public async Task<string> UploadImageFromFormAsync(IFormFile imageFile, Guid entityId)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                {
                    Console.WriteLine("Файл не знайдено або він порожній.");
                    return null;
                }

                var fileName = $"{entityId}{Path.GetExtension(imageFile.FileName)}";

                // Завантажуємо файл в storage
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
                    formFile,
                    "book/images/",
                    entityId);

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
