using BookApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherService _publisherService;
        private const int DefaultPageNumber = 1;
        private const int DefaultPageSize = 10;
        public PublishersController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublisherDto>>> GetPublishers(int pageNumber = DefaultPageNumber, int pageSize = DefaultPageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than 0.");
            }

            var publishers = await _publisherService.GetPublishersAsync();

            if (publishers == null || !publishers.Any())
            {
                return NotFound("No publishers found.");
            }

            var paginated = publishers
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(paginated);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PublisherDto>> GetPublisherById(Guid id)
        {
            var publisher = await _publisherService.GetPublisherByIdAsync(id);

            if (publisher == null)
            {
                return NotFound($"Publisher with id {id} not found.");
            }

            return Ok(publisher);
        }
        [HttpPost]
        public async Task<ActionResult<PublisherDto>> CreatePublisher([FromBody] PublisherDto publisherDto)
        {
            if (publisherDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var created = await _publisherService.CreatePublisherAsync(publisherDto);

            return CreatedAtAction(nameof(GetPublisherById), new { id = created.PublisherId }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PublisherDto>> UpdatePublisher(Guid id, [FromBody] PublisherDto publisherDto)
        {
            if (publisherDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var updated = await _publisherService.UpdatePublisherAsync(id, publisherDto);

            if (updated == null)
            {
                return NotFound("Publisher not found.");
            }

            return Ok(updated);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePublisher(Guid id)
        {
            var isDeleted = await _publisherService.DeletePublisherAsync(id);

            if (!isDeleted)
            {
                return NotFound("Publisher not found.");
            }

            return NoContent();
        }
    }

}