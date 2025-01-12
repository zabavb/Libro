using BookApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherService _publisherService;
        private readonly ILogger<PublishersController> _logger;
        private const int DefaultPageNumber = 1;
        private const int DefaultPageSize = 10;

        public PublishersController(IPublisherService publisherService, ILogger<PublishersController> logger)
        {
            _publisherService = publisherService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublisherDto>>> GetPublishers(int pageNumber = DefaultPageNumber, int pageSize = DefaultPageSize)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                {
                    _logger.LogWarning("Invalid page number or page size.");
                    return BadRequest("Page number and page size must be greater than 0.");
                }

                var publishers = await _publisherService.GetPublishersAsync();

                if (publishers == null || !publishers.Any())
                {
                    _logger.LogInformation("No publishers found.");
                    return NotFound("No publishers found.");
                }

                var paginated = publishers
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(paginated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving publishers.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PublisherDto>> GetPublisherById(Guid id)
        {
            try
            {
                var publisher = await _publisherService.GetPublisherByIdAsync(id);

                if (publisher == null)
                {
                    _logger.LogWarning($"Publisher with id {id} not found.");
                    return NotFound($"Publisher with id {id} not found.");
                }

                _logger.LogInformation($"Publisher with id {id} successfully fetched.");
                return Ok(publisher);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving publisher with id {id}.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PublisherDto>> CreatePublisher([FromBody] PublisherDto publisherDto)
        {
            try
            {
                if (publisherDto == null)
                {
                    _logger.LogWarning("Invalid data provided for creating publisher.");
                    return BadRequest("Invalid data.");
                }

                var created = await _publisherService.CreatePublisherAsync(publisherDto);
                _logger.LogInformation($"Publisher with id {created.PublisherId} successfully created.");

                return CreatedAtAction(nameof(GetPublisherById), new { id = created.PublisherId }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new publisher.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PublisherDto>> UpdatePublisher(Guid id, [FromBody] PublisherDto publisherDto)
        {
            try
            {
                if (publisherDto == null)
                {
                    _logger.LogWarning("Invalid data provided for updating publisher.");
                    return BadRequest("Invalid data.");
                }

                var updated = await _publisherService.UpdatePublisherAsync(id, publisherDto);

                if (updated == null)
                {
                    _logger.LogWarning($"Publisher with id {id} not found for update.");
                    return NotFound("Publisher not found.");
                }

                _logger.LogInformation($"Publisher with id {id} successfully updated.");
                return Ok(updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating publisher with id {id}.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePublisher(Guid id)
        {
            try
            {
                var isDeleted = await _publisherService.DeletePublisherAsync(id);

                if (!isDeleted)
                {
                    _logger.LogWarning($"Publisher with id {id} not found for deletion.");
                    return NotFound("Publisher not found.");
                }

                _logger.LogInformation($"Publisher with id {id} successfully deleted.");
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
