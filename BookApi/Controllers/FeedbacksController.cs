using BookAPI.Services;
using FeedbackApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        private const int DefaultPageNumber = 1;
        private const int DefaultPageSize = 10;
        public FeedbacksController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackDto>>> GetFeedbacks(int pageNumber = DefaultPageNumber, int pageSize = DefaultPageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than 0.");
            }

            var feedbacks = await _feedbackService.GetFeedbacksAsync();

            if (feedbacks == null || !feedbacks.Any())
            {
                return NotFound("No feedbacks found.");
            }

            var paginated = feedbacks
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(paginated);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackDto>> GetFeedbackById(Guid id)
        {
            var feedback = await _feedbackService.GetFeedbackByIdAsync(id);

            if (feedback == null)
            {
                return NotFound($"Feedback with id {id} not found.");
            }

            return Ok(feedback);
        }
        [HttpPost]
        public async Task<ActionResult<FeedbackDto>> CreateFeedback([FromBody] FeedbackDto feedbackDto)
        {
            if (feedbackDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var created = await _feedbackService.CreateFeedbackAsync(feedbackDto);

            return CreatedAtAction(nameof(GetFeedbackById), new { id = created.FeedbackId }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FeedbackDto>> UpdateFeedback(Guid id, [FromBody] FeedbackDto feedbackDto)
        {
            if (feedbackDto == null)
            {
                return BadRequest("Invalid data.");
            }

            var updated = await _feedbackService.UpdateFeedbackAsync(id, feedbackDto);

            if (updated == null)
            {
                return NotFound("feedback not found.");
            }

            return Ok(updated);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFeedback(Guid id)
        {
            var isDeleted = await _feedbackService.DeleteFeedbackAsync(id);

            if (!isDeleted)
            {
                return NotFound("feedback not found.");
            }

            return NoContent();
        }
    }
}