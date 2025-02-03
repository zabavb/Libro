using BookAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookAPI.Controllers
{
    /// <summary>
    /// Controller for retrieving reference data such as cover types and languages.
    /// </summary>
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

        /// <summary>
        /// Retrieves a list of cover types.
        /// </summary>
        /// <returns>A list of cover types.</returns>
        /// <response code="200">Returns the list of cover types.</response>
        /// <response code="404">If no cover types are found.</response>
        /// <response code="500">If an error occurs while retrieving cover types.</response>
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

        /// <summary>
        /// Retrieves a specific cover type by its ID.
        /// </summary>
        /// <param name="id">The ID of the cover type.</param>
        /// <returns>The cover type corresponding to the given ID.</returns>
        /// <response code="200">Returns the cover type with the specified ID.</response>
        /// <response code="404">If the cover type with the specified ID is not found.</response>
        /// <response code="500">If an error occurs while retrieving the cover type.</response>
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

        /// <summary>
        /// Retrieves a list of languages.
        /// </summary>
        /// <returns>A list of languages.</returns>
        /// <response code="200">Returns the list of languages.</response>
        /// <response code="404">If no languages are found.</response>
        /// <response code="500">If an error occurs while retrieving languages.</response>
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

        /// <summary>
        /// Retrieves a specific language by its ID.
        /// </summary>
        /// <param name="id">The ID of the language.</param>
        /// <returns>The language corresponding to the given ID.</returns>
        /// <response code="200">Returns the language with the specified ID.</response>
        /// <response code="404">If the language with the specified ID is not found.</response>
        /// <response code="500">If an error occurs while retrieving the language.</response>
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
