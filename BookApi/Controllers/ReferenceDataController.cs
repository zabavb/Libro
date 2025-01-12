using BookAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferenceDataController : ControllerBase
    {
        private readonly ICoverTypeService _coverTypeService;
        private readonly ILanguageService _languageService;
        private readonly ILogger<ReferenceDataController> _logger;

        public ReferenceDataController(ICoverTypeService coverTypeService, ILanguageService languageService, ILogger<ReferenceDataController> logger)
        {
            _coverTypeService = coverTypeService;
            _languageService = languageService;
            _logger = logger;
        }

        [HttpGet("cover-types")]
        public async Task<ActionResult<IEnumerable<string>>> GetCoverTypes()
        {
            try
            {
                var coverTypes = await _coverTypeService.GetCoverTypesAsync();
                if (coverTypes == null || !coverTypes.Any())
                {
                    _logger.LogInformation("No cover types found.");
                    return NotFound("No cover types found.");
                }

                _logger.LogInformation("Successfully fetched cover types.");
                return Ok(coverTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving cover types.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("cover-types/{id}")]
        public async Task<ActionResult<string>> GetCoverType(int id)
        {
            try
            {
                var coverType = await _coverTypeService.GetCoverTypeByIdAsync(id);
                if (coverType == null)
                {
                    _logger.LogWarning($"Cover type with id {id} not found.");
                    return NotFound($"Cover type with id {id} not found.");
                }

                _logger.LogInformation($"Successfully fetched cover type with id {id}.");
                return Ok(coverType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving cover type with id {id}.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("languages")]
        public async Task<ActionResult<IEnumerable<string>>> GetLanguages()
        {
            try
            {
                var languages = await _languageService.GetLanguagesAsync();
                if (languages == null || !languages.Any())
                {
                    _logger.LogInformation("No languages found.");
                    return NotFound("No languages found.");
                }

                _logger.LogInformation("Successfully fetched languages.");
                return Ok(languages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving languages.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("languages/{id}")]
        public async Task<ActionResult<string>> GetLanguage(int id)
        {
            try
            {
                var language = await _languageService.GetLanguageByIdAsync(id);
                if (language == null)
                {
                    _logger.LogWarning($"Language with id {id} not found.");
                    return NotFound($"Language with id {id} not found.");
                }

                _logger.LogInformation($"Successfully fetched language with id {id}.");
                return Ok(language);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving language with id {id}.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
