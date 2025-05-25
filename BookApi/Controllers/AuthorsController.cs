using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using BookAPI.Services.Interfaces;
using Library.Common;
using Microsoft.AspNetCore.Mvc;

namespace BookAPI.Controllers
{
    /// <summary>
    /// Manages author-related operations such as retrieving, creating, updating, and deleting authors.
    /// </summary>
    /// <remarks>
    /// This controller provides CRUD operations for managing authors in the system.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(IAuthorService authorService, ILogger<AuthorsController> logger)
        {
            _authorService = authorService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a paginated list of authors.
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1). The page number to retrieve.</param>
        /// <param name="pageSize">Number of authors per page (default: 10). The number of authors to return per page.</param>
        /// <param name="searchTerm">Search term (optional). A string to search in the author's name or other properties.</param>
        /// <param name="filter">Filter options (optional). An object containing criteria to filter the authors by.</param>
        /// <param name="sort">Sort options (optional). An object containing sorting preferences for the authors.</param>
        /// <returns>A paginated list of authors.</returns>
        /// <response code="200">Returns a list of authors.</response>
        /// <response code="400">If the page number or page size is invalid.</response>
        /// <response code="404">If no authors are found.</response>
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<AuthorDto>>> GetAll(
            [FromQuery] int pageNumber = GlobalConstants.DefaultPageNumber,
            [FromQuery] int pageSize = GlobalConstants.DefaultPageSize,
            [FromQuery] string? searchTerm = null,
            [FromQuery] AuthorFilter? filter = null,
            [FromQuery] AuthorSort? sort = null
        )
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                _logger.LogWarning("Invalid page number or page size.");
                return BadRequest("Page number and page size must be greater than 0.");
            }

            try
            {
                var authors = await _authorService.GetAllAsync(pageNumber, pageSize, searchTerm, filter, sort);

                if (authors == null || authors.Items == null)
                {
                    _logger.LogInformation("No authors found.");
                    return NotFound("No authors found.");
                }

                _logger.LogInformation("Authors successfully fetched.");
                return Ok(authors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching authors.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves an author by their unique ID.
        /// </summary>
        /// <param name="id">The unique identifier of the author.</param>
        /// <returns>The author with the specified ID.</returns>
        /// <response code="200">Returns the requested author.</response>
        /// <response code="404">If the author is not found.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetById(Guid id)
        {
            try
            {
                var author = await _authorService.GetByIdAsync(id);

                if (author == null)
                {
                    return NotFound($"Author with id {id} not found.");
                }

                return Ok(author);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Creates a new author.
        /// </summary>
        /// <param name="authorDto">The author details.</param>
        /// <returns>The created author.</returns>
        /// <response code="201">Returns the newly created author.</response>
        /// <response code="400">If the provided data is invalid.</response>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<AuthorDto>> CreateAuthor([FromForm] AuthorRequest authorDto)
        {
            if (authorDto == null)
            {
                _logger.LogWarning("Invalid author data provided.");
                return BadRequest("Invalid data.");
            }

            try
            {
                /*var created = */
                await _authorService.CreateAsync(authorDto);
                // return CreatedAtAction(nameof(GetById), new { id = created.AuthorId }, created);
                return CreatedAtAction(nameof(GetById), new { id = authorDto.AuthorId }, authorDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new author.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing author.
        /// </summary>
        /// <param name="id">The unique identifier of the author.</param>
        /// <param name="authorDto">Updated author details.</param>
        /// <returns>The updated author.</returns>
        /// <response code="200">Returns the updated author.</response>
        /// <response code="400">If the provided data is invalid.</response>
        /// <response code="404">If the author is not found.</response>
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<AuthorDto>> Update(Guid id, [FromForm] AuthorRequest authorDto)
        {
            if (authorDto == null)
            {
                _logger.LogWarning("Invalid author data provided for update.");
                return BadRequest("Invalid data.");
            }

            try
            {
                /*var updated = */
                await _authorService.UpdateAsync(id, authorDto);

                /*if (updated == null)
                {
                    return NotFound($"Author with id {id} not found.");
                }*/

                return Ok( /*updated*/);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating author with id {id}.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes an author by their unique ID.
        /// </summary>
        /// <param name="id">The unique identifier of the author.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Indicates successful deletion.</response>
        /// <response code="404">If the author is not found.</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuthor(Guid id)
        {
            try
            {
                /*var isDeleted = */
                await _authorService.DeleteAsync(id);

                /*if (!isDeleted)
                {
                    return NotFound($"Author with id {id} not found.");
                }*/

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}