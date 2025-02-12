using BookAPI;
using BookAPI.Services.Interfaces;
using Library.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookAPI.Controllers
{
    /// <summary>
    /// Controller for managing book categories.
    /// </summary>
    /// <remarks>
    /// This controller provides CRUD operations for managing categories in the system.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a paginated list of categories.
        /// </summary>
        /// <param name="pageNumber">The page number (default is 1).</param>
        /// <param name="pageSize">The number of items per page (default is 10).</param>
        /// <returns>A response containing a paginated list of categories.</returns>
        /// <response code="200">Returns a list of categories.</response>
        /// <response code="400">Invalid pagination parameters.</response>
        /// <response code="404">No categories found.</response>
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<CategoryDto>>> GetCategories(
            [FromQuery] int pageNumber = GlobalConstants.DefaultPageNumber,
            [FromQuery] int pageSize = GlobalConstants.DefaultPageSize
            )
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                {
                    _logger.LogWarning("Invalid page number or page size.");
                    return BadRequest("Page number and page size must be greater than 0.");
                }

                var categories = await _categoryService.GetCategoriesAsync(pageNumber, pageSize);

                if (categories == null || categories.Items == null || !categories.Items.Any())
                {
                    _logger.LogInformation("No categories found.");
                    return NotFound("No categories found.");
                }

                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving categories.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a category by its ID.
        /// </summary>
        /// <param name="id">The category ID.</param>
        /// <returns>A response containing the category details.</returns>
        /// <response code="200">Returns the category.</response>
        /// <response code="404">Category not found.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(Guid id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);

                if (category == null)
                {
                    _logger.LogWarning($"Category with id {id} not found.");
                    return NotFound($"Category with id {id} not found.");
                }

                _logger.LogInformation($"Category with id {id} successfully fetched.");
                return Ok(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving category with id {id}.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="categoryDto">The category details.</param>
        /// <returns>A response containing the created category.</returns>
        /// <response code="201">Category successfully created.</response>
        /// <response code="400">Invalid data provided.</response>
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CategoryDto categoryDto)
        {
            try
            {
                if (categoryDto == null)
                {
                    _logger.LogWarning("Invalid data provided for creating category.");
                    return BadRequest("Invalid data.");
                }

                var created = await _categoryService.CreateCategoryAsync(categoryDto);
                _logger.LogInformation($"Category with id {created.CategoryId} successfully created.");

                return CreatedAtAction(nameof(GetCategoryById), new { id = created.CategoryId }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new category.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="id">The category ID.</param>
        /// <param name="categoryDto">The updated category details.</param>
        /// <returns>A response containing the updated category.</returns>
        /// <response code="200">Category successfully updated.</response>
        /// <response code="400">Invalid data provided.</response>
        /// <response code="404">Category not found.</response>
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(Guid id, [FromBody] CategoryDto categoryDto)
        {
            try
            {
                if (categoryDto == null)
                {
                    _logger.LogWarning("Invalid data provided for updating category.");
                    return BadRequest("Invalid data.");
                }

                var updated = await _categoryService.UpdateCategoryAsync(id, categoryDto);

                if (updated == null)
                {
                    _logger.LogWarning($"Category with id {id} not found for update.");
                    return NotFound("Category not found.");
                }

                _logger.LogInformation($"Category with id {id} successfully updated.");
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating category with id {id}.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a category by its ID.
        /// </summary>
        /// <param name="id">The category ID.</param>
        /// <returns>A response indicating success or failure.</returns>
        /// <response code="204">Category successfully deleted.</response>
        /// <response code="404">Category not found.</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(Guid id)
        {
            try
            {
                var isDeleted = await _categoryService.DeleteCategoryAsync(id);

                if (!isDeleted)
                {
                    _logger.LogWarning($"Category with id {id} not found for deletion.");
                    return NotFound("Category not found.");
                }

                _logger.LogInformation($"Category with id {id} successfully deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting category with id {id}.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
