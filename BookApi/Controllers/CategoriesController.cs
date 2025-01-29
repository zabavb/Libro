﻿using BookApi.Services;
using Library.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;
        private const int DefaultPageNumber = 1;
        private const int DefaultPageSize = 10;

        public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<CategoryDto>>> GetCategories(int pageNumber = DefaultPageNumber, int pageSize = DefaultPageSize)
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
