﻿using BookAPI.Models.Filters;
using BookAPI.Models.Sortings;
using BookAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Library.Common;
using Library.DTOs.UserRelated.User;
using Microsoft.AspNetCore.Authorization;
using BookAPI.Services;
using Library.DTOs.Order;

namespace BookAPI.Controllers
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
        /// <param name="pageNumber">Page number (default: 1). The page number to retrieve.</param>
        /// <param name="pageSize">Number of feedbacks per page (default: 10). The number of feedbacks to return per page.</param>
        /// <param name="filter">Filter options (optional). An object containing criteria to filter the feedbacks by.</param>
        /// <param name="sort">Sort options (optional). An object containing sorting preferences for the feedbacks.</param>
        /// <returns>A paginated list of feedbacks.</returns>
        /// <response code="200">Returns a list of feedbacks according to the specified pagination parameters.</response>
        /// <response code="400">Returns an error if the page number or page size is invalid.</response>
        /// <response code="404">Returns an error if no feedbacks are found.</response>
        /// <response code="500">Returns an internal server error if an exception occurs.</response>
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<FeedbackDto>>> GetAll(
            [FromQuery] int pageNumber = GlobalConstants.DefaultPageNumber,
            [FromQuery] int pageSize = GlobalConstants.DefaultPageSize,
            [FromQuery] FeedbackFilter? filter = null,
            [FromQuery] FeedbackSort? sort = null
        )
        {
            try
            {
                if (pageNumber < 1 || pageSize < 1)
                {
                    _logger.LogWarning("Invalid page number or page size.");
                    return BadRequest("Page number and page size must be greater than 0.");
                }

                var feedbacks = await _feedbackService.GetAllAsync(pageNumber, pageSize, filter, sort);

                if (feedbacks == null || feedbacks.Items == null || !feedbacks.Items.Any())
                {
                    return NotFound("No feedbacks found.");
                }

                _logger.LogInformation("Feedbacks successfully fetched.");
                return Ok(feedbacks);
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
        /// <param name="id">The ID of the feedback to retrieve.</param>
        /// <returns>The requested feedback.</returns>
        /// <response code="200">Returns the feedback with the specified ID.</response>
        /// <response code="404">Returns an error if the feedback with the specified ID is not found.</response>
        /// <response code="500">Returns an internal server error if an exception occurs.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackDto>> GetById(Guid id)
        {
            try
            {
                var feedback = await _feedbackService.GetByIdAsync(id);

                if (feedback == null)
                {
                    return NotFound($"Feedback with id {id} not found.");
                }

                return Ok(feedback);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving feedback with id {id}.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves Feedbacks data for user's details page by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>Snippets of feedbacks data which user ID matches with provided one in parameters.</returns>
        /// <response code="200">Retrieval successful, return the feedback snippets.</response>
        /// <response code="404">Could not find the user.</response>
        /// <response code="500">An unexpected error occured.</response>
        [Authorize(Roles = "ADMIN, MODERATOR")]
        [HttpGet("for-user/details/{id}")]
        public async Task<ActionResult<ICollection<FeedbackForUserDetails>>> GetAllForUserDetailsAsync(Guid id)
        {
            if (id == Guid.Empty)
                return NotFound($"User ID [{id}] was not provided.");

            var snippets = await _feedbackService.GetAllForUserDetailsAsync(id);
            return Ok(snippets);
        }

        /// <summary>
        /// Creates a new feedback.
        /// </summary>
        /// <param name="feedbackDto">The feedback data to create.</param>
        /// <returns>The created feedback.</returns>
        /// <response code="201">Returns the created feedback.</response>
        /// <response code="400">Returns an error if the provided data is invalid.</response>
        /// <response code="500">Returns an internal server error if an exception occurs.</response>
        [HttpPost]
        public async Task<ActionResult<FeedbackDto>> Create([FromBody] FeedbackDto feedbackDto)
        {
            if (feedbackDto == null)
            {
                _logger.LogWarning("Invalid feedback data provided.");
                return BadRequest("Invalid data.");
            }

            try
            {
                /*var createdFeedback = */
                await _feedbackService.CreateAsync(feedbackDto);
                return CreatedAtAction(nameof(GetById), new { id = feedbackDto.FeedbackId }, feedbackDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating feedback.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


        /*/// <summary>
        /// Updates an existing feedback.
        /// </summary>
        /// <param name="id">The ID of the feedback to update.</param>
        /// <param name="feedbackDto">The updated feedback data.</param>
        /// <returns>The updated feedback.</returns>
        /// <response code="200">Returns the updated feedback.</response>
        /// <response code="400">Returns an error if the provided data is invalid.</response>
        /// <response code="404">Returns an error if the feedback with the specified ID is not found.</response>
        /// <response code="500">Returns an internal server error if an exception occurs.</response>
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
        }*/

        /// <summary>
        /// Deletes a feedback by its ID.
        /// </summary>
        /// <param name="id">The ID of the feedback to delete.</param>
        /// <returns>No content if the deletion is successful.</returns>
        /// <response code="204">Returns no content if the feedback is successfully deleted.</response>
        /// <response code="404">Returns an error if the feedback with the specified ID is not found for deletion.</response>
        /// <response code="500">Returns an internal server error if an exception occurs.</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                /*var isDeleted = */
                await _feedbackService.DeleteAsync(id);

                /*if (!isDeleted)
                {
                    return NotFound($"Feedback with id {id} not found.");
                }*/

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting feedback with id {id}.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

/*        [HttpGet("for-book/rating/{id}")]
        public async Task<ActionResult<BookFeedbacks>> GetAllForOrderDetailsAsync(Guid id)
        {
            if (id == Guid.Empty)
                return NotFound($"Feedback ID [{id}] was not provided.");
            try
            {
                var snippet = await _feedbackService.GetBookAvgRating(id);
                return Ok(snippet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }*/
    }
}