﻿using BookApi.Services;
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
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors(int pageNumber = DefaultPageNumber, int pageSize = DefaultPageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                _logger.LogWarning("Invalid page number or page size.");
                return BadRequest("Page number and page size must be greater than 0.");
            }

            try
            {
                var authors = await _authorService.GetAuthorsAsync();

                if (authors == null || !authors.Any())
                { 
                    return NotFound("No authors found.");
                }

                var paginated = authors
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                _logger.LogInformation("Authors successfully fetched.");
                return Ok(paginated);
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
                    return NotFound($"Author with id {id} not found.");
                }

                return Ok(author);
            }
            catch (Exception ex)
            {
                
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
                return CreatedAtAction(nameof(GetAuthorById), new { id = created.AuthorId }, created);
            }
            catch (Exception ex)
            {
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
                    return NotFound($"Author with id {id} not found.");
                }

                return Ok(updated);
            }
            catch (Exception ex)
            {
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
                    return NotFound($"Author with id {id} not found.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
