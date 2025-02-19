using BookApi.Models;
using BookAPI.Services;
using FeedbackApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{
    /// <summary>
    /// Manages feedback-related operations such as retrieving, creating, updating, and deleting feedbacks.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        private readonly ILogger<FeedbacksController> _logger;

        public FeedbacksController(IFeedbackService feedbackService, ILogger<FeedbacksController> logger)
        {
            _feedbackService = feedbackService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a paginated list of feedbacks.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetFeedbacks([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                {
                    _logger.LogWarning("Invalid page number or page size.");
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving feedbacks.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a feedback by its ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackDto>> GetFeedbackById(Guid id)
        {
            try
            {
                var feedback = await _feedbackService.GetFeedbackByIdAsync(id);

                if (feedback == null)
                {
                    return NotFound($"Feedback with id {id} not found.");
                }

                return Ok(feedback);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving feedback with id {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Creates a new feedback.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<FeedbackDto>> CreateFeedback([FromBody] FeedbackDto feedbackDto)
        {
            if (feedbackDto == null)
            {
                _logger.LogWarning("Invalid feedback data provided.");
                return BadRequest("Invalid data.");
            }

            try
            {
                var createdFeedback = await _feedbackService.CreateFeedbackAsync(feedbackDto);
                return CreatedAtAction(nameof(GetFeedbackById), new { id = createdFeedback.FeedbackId }, createdFeedback);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating feedback.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing feedback.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<FeedbackDto>> UpdateFeedback(Guid id, [FromBody] FeedbackDto feedbackDto)
        {
            if (feedbackDto == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var updatedFeedback = await _feedbackService.UpdateFeedbackAsync(id, feedbackDto);

                if (updatedFeedback == null)
                {
                    return NotFound($"Feedback with id {id} not found.");
                }

                return Ok(updatedFeedback);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating feedback with id {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a feedback by its ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFeedback(Guid id)
        {
            try
            {
                var isDeleted = await _feedbackService.DeleteFeedbackAsync(id);

                if (!isDeleted)
                {
                    return NotFound($"Feedback with id {id} not found.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting feedback with id {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
