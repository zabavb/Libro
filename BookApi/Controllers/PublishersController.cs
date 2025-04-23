using BookAPI.Models.Sortings;
using BookAPI.Services.Interfaces;
using Library.Common;
using Microsoft.AspNetCore.Mvc;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherService _publisherService;
        private readonly ILogger<PublishersController> _logger;

        public PublishersController(IPublisherService publisherService, ILogger<PublishersController> logger)
        {
            _publisherService = publisherService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a paginated list of publishers.
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1). The page number to retrieve.</param>
        /// <param name="pageSize">Number of publishers per page (default: 10). The number of publishers to return per page.</param>
        /// <param name="searchTerm">Search term (optional). A string to search in the publisher's name or other properties.</param>
        /// <param name="sort">Sort options (optional). An object containing sorting preferences for the publishers.</param>
        /// <returns>A paginated list of publishers.</returns>
        /// <response code="200">Returns a list of publishers according to the specified pagination parameters.</response>
        /// <response code="400">Returns if the page number or page size is less than 1.</response>
        /// <response code="404">Returns if no publishers are found.</response>
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<PublisherDto>>> GetPublishers(
            [FromQuery] int pageNumber = GlobalConstants.DefaultPageNumber,
            [FromQuery] int pageSize = GlobalConstants.DefaultPageSize,
            [FromQuery] string? searchTerm = null,
            [FromQuery] PublisherSort? sort = null
        )
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                {
                    _logger.LogWarning("Invalid page number or page size.");
                    return BadRequest("Page number and page size must be greater than 0.");
                }

                var publishers = await _publisherService.GetAllAsync(pageNumber, pageSize, searchTerm, sort);

                if (publishers == null || publishers.Items == null || publishers.Items.Count == 0)
                {
                    return NotFound("No publishers found.");
                }

                _logger.LogInformation("Publishers successfully fetched.");
                return Ok(publishers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving publishers.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


        /// <summary>
        /// Retrieves a publisher by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the publisher.</param>
        /// <returns>A publisher object.</returns>
        /// <response code="200">Returns the publisher with the specified ID.</response>
        /// <response code="404">Returns if no publisher is found with the specified ID.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<PublisherDto>> GetById(Guid id)
        {
            try
            {
                var publisher = await _publisherService.GetByIdAsync(id);

                if (publisher == null)
                {
                    return NotFound($"Publisher with id {id} not found.");
                }

                return Ok(publisher);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving publisher with id {id}.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new publisher.
        /// </summary>
        /// <param name="publisherDto">The publisher data to be created.</param>
        /// <returns>The created publisher object.</returns>
        /// <response code="201">Returns the newly created publisher.</response>
        /// <response code="400">Returns if the provided data is invalid.</response>
        [HttpPost]
        public async Task<ActionResult<PublisherDto>> Create([FromBody] PublisherDto publisherDto)
        {
            try
            {
                if (publisherDto == null)
                {
                    _logger.LogWarning("Invalid data provided for creating publisher.");
                    return BadRequest("Invalid data.");
                }

                /*var created = */
                await _publisherService.CreateAsync(publisherDto);

                return CreatedAtAction(nameof(GetById), new { id = publisherDto.PublisherId }, publisherDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new publisher.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing publisher.
        /// </summary>
        /// <param name="id">The unique identifier of the publisher to update.</param>
        /// <param name="publisherDto">The updated publisher data.</param>
        /// <returns>The updated publisher object.</returns>
        /// <response code="200">Returns the updated publisher.</response>
        /// <response code="400">Returns if the provided data is invalid.</response>
        /// <response code="404">Returns if no publisher is found with the specified ID.</response>
        [HttpPut("{id}")]
        public async Task<ActionResult<PublisherDto>> Update(Guid id, [FromBody] PublisherDto publisherDto)
        {
            try
            {
                if (publisherDto == null)
                {
                    _logger.LogWarning("Invalid data provided for updating publisher.");
                    return BadRequest("Invalid data.");
                }

                /*var updated = */
                await _publisherService.UpdateAsync(publisherDto);

                /*if (updated == null)
                {
                    return NotFound("Publisher not found.");
                }*/

                return Ok( /*updated*/);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating publisher with id {id}.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a publisher by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the publisher to delete.</param>
        /// <returns>No content response if the deletion is successful.</returns>
        /// <response code="204">Returns if the publisher is successfully deleted.</response>
        /// <response code="404">Returns if no publisher is found with the specified ID.</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                /*var isDeleted = */
                await _publisherService.DeleteAsync(id);

                /*if (!isDeleted)
                {
                    return NotFound("Publisher not found.");
                }*/

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting publisher with id {id}.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}