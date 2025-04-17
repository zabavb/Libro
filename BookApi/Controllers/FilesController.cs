using Library.Common;
using Microsoft.AspNetCore.Mvc;

namespace BookAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly S3StorageService _storageService;

        public FilesController(S3StorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpGet("signed-url")]
        public IActionResult GetSignedUrl([FromQuery] string fileKey)
        {
            //return await _storageService.UploadAsync(_bucketName, image, folder, id);
            try
            {
                var signedUrl = _storageService.GenerateSignedUrl(GlobalConstants.bucketName,fileKey);
                return Ok(new { SignedUrl = signedUrl });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
