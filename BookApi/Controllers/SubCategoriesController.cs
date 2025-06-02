using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using BookAPI.Services.Interfaces;
using Library.Common;
using Microsoft.AspNetCore.Mvc;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoriesController : ControllerBase
    {
        private readonly ISubCategoryService _subCategoryService;
        private readonly ILogger<SubCategoriesController> _logger;

        public SubCategoriesController(ISubCategoryService subCategoryService, ILogger<SubCategoriesController> logger)
        {
            _subCategoryService = subCategoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<SubCategoryDto>>> GetAll(
            [FromQuery] int pageNumber = GlobalConstants.DefaultPageNumber,
            [FromQuery] int pageSize = GlobalConstants.DefaultPageSize,
            [FromQuery] string? searchTerm = null,
            [FromQuery] SubCategoryFilter? filter = null,
            [FromQuery] SubCategorySort? sort = null
        )
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                {
                    _logger.LogWarning("Invalid page number or page size.");
                    return BadRequest("Page number and page size must be greater than 0.");
                }

                var subCategories =
                    await _subCategoryService.GetAllAsync(pageNumber, pageSize, searchTerm, filter, sort);

                if (subCategories == null || subCategories.Items == null)
                {
                    _logger.LogInformation("No subcategories found.");
                    return NotFound("No subcategories found.");
                }

                return Ok(subCategories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving subcategories.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<SubCategoryDto>> GetById(Guid id)
        {
            try
            {
                var subCategory = await _subCategoryService.GetByIdAsync(id);

                if (subCategory == null)
                {
                    _logger.LogWarning($"Subcategory with id {id} not found.");
                    return NotFound($"Subcategory with id {id} not found.");
                }

                _logger.LogInformation($"Subcategory with id {id} successfully fetched.");
                return Ok(subCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving subcategory with id {id}.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<SubCategoryDto>> Create([FromBody] SubCategoryDto subCategoryDto)
        {
            try
            {
                if (subCategoryDto == null)
                {
                    _logger.LogWarning("Invalid data provided for creating subcategory.");
                    return BadRequest("Invalid data.");
                }

                /*var created = */
                await _subCategoryService.CreateAsync(subCategoryDto);
                _logger.LogInformation($"Subcategory with id {subCategoryDto.SubCategoryId} successfully created.");

                return CreatedAtAction(nameof(GetById), new { id = subCategoryDto.SubCategoryId }, subCategoryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new subcategory.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SubCategoryDto>> Update(Guid id,
            [FromBody] SubCategoryDto subCategoryDto)
        {
            try
            {
                if (subCategoryDto == null)
                {
                    _logger.LogWarning("Invalid data provided for updating subcategory.");
                    return BadRequest("Invalid data.");
                }

                /*var updated = */
                await _subCategoryService.UpdateAsync(subCategoryDto);

                /*if (updated == null)
                {
                    _logger.LogWarning($"Subcategory with id {id} not found for update.");
                    return NotFound("Subcategory not found.");
                }*/

                _logger.LogInformation($"Subcategory with id {id} successfully updated.");
                return Ok( /*updated*/);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating subcategory with id {id}.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSubCategory(Guid id)
        {
            try
            {
                /*var isDeleted = */
                await _subCategoryService.DeleteAsync(id);

                /*if (!isDeleted)
                {
                    _logger.LogWarning($"Subcategory with id {id} not found for deletion.");
                    return NotFound("Subcategory not found.");
                }*/

                _logger.LogInformation($"Subcategory with id {id} successfully deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting subcategory with id {id}.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}