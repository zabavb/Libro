using BookAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferenceDataController : ControllerBase
    {
        private readonly ICoverTypeService _coverTypeService;
        private readonly ILanguageService _languageService;

        public ReferenceDataController(ICoverTypeService coverTypeService, ILanguageService languageService)
        {
            _coverTypeService = coverTypeService;
            _languageService = languageService;
        }

        [HttpGet("cover-types")]
        public async Task<ActionResult<IEnumerable<string>>> GetCoverTypes()
        {
            var coverTypes = await _coverTypeService.GetCoverTypesAsync();
            return Ok(coverTypes);
        }

        [HttpGet("cover-types/{id}")]
        public async Task<ActionResult<string>> GetCoverType(int id)
        {
            var coverType = await _coverTypeService.GetCoverTypeByIdAsync(id);
            if (coverType == null) return NotFound();
            return Ok(coverType);
        }

        [HttpGet("languages")]
        public async Task<ActionResult<IEnumerable<string>>> GetLanguages()
        {
            var languages = await _languageService.GetLanguagesAsync();
            return Ok(languages);
        }

        [HttpGet("languages/{id}")]
        public async Task<ActionResult<string>> GetLanguage(int id)
        {
            var language = await _languageService.GetLanguageByIdAsync(id);
            if (language == null) return NotFound();
            return Ok(language);
        }
    }
}
