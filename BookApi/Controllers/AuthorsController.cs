using BookApi.Services;
using Library.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly ILogger<AuthorsController> _logger;
        private const int DefaultPageNumber = 1;
        private const int DefaultPageSize = 10;

        public AuthorsController(IAuthorService authorService, ILogger<AuthorsController> logger)
        {
            _authorService = authorService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<AuthorDto>>> GetAuthors(int pageNumber = DefaultPageNumber, int pageSize = DefaultPageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                _logger.LogWarning("Invalid page number or page size.");
                return BadRequest("Page number and page size must be greater than 0.");
            }

            try
            {
                var authors = await _authorService.GetAuthorsAsync(pageNumber, pageSize);

                if (authors == null || authors.Items == null || !authors.Items.Any())
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

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetAuthorById(Guid id)
        {
            try
            {
                var author = await _authorService.GetAuthorByIdAsync(id);

                if (author == null)
                {
                    _logger.LogWarning($"Author with id {id} not found.");
                    return NotFound($"Author with id {id} not found.");
                }

                _logger.LogInformation($"Author with id {id} successfully fetched.");
                return Ok(author);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching author with id {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<AuthorDto>> CreateAuthor([FromBody] AuthorDto authorDto)
        {
            if (authorDto == null)
            {
                _logger.LogWarning("Invalid author data provided.");
                return BadRequest("Invalid data.");
            }

            try
            {
                var created = await _authorService.CreateAuthorAsync(authorDto);
                _logger.LogInformation($"Author with id {created.AuthorId} successfully created.");
                return CreatedAtAction(nameof(GetAuthorById), new { id = created.AuthorId }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new author.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AuthorDto>> UpdateAuthor(Guid id, [FromBody] AuthorDto authorDto)
        {
            if (authorDto == null)
            {
                _logger.LogWarning("Invalid author data provided for update.");
                return BadRequest("Invalid data.");
            }

            try
            {
                var updated = await _authorService.UpdateAuthorAsync(id, authorDto);

                if (updated == null)
                {
                    _logger.LogWarning($"Author with id {id} not found for update.");
                    return NotFound($"Author with id {id} not found.");
                }

                _logger.LogInformation($"Author with id {id} successfully updated.");
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating author with id {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuthor(Guid id)
        {
            try
            {
                var isDeleted = await _authorService.DeleteAuthorAsync(id);

                if (!isDeleted)
                {
                    _logger.LogWarning($"Author with id {id} not found for deletion.");
                    return NotFound($"Author with id {id} not found.");
                }

                _logger.LogInformation($"Author with id {id} successfully deleted.");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting author with id {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
