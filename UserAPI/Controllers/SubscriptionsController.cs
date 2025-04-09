using BookAPI;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Services.Interfaces;
using Library.Common;
using Library.DTOs.UserRelated.Subscription;
using Microsoft.AspNetCore.Authorization;
using UserAPI.Models.Subscription;

namespace UserAPI.Controllers
{
    /// <summary>
    /// Controller for managing subscription-related operations.
    /// </summary>
    /// <remarks>
    /// This controller provides endpoints for performing CRUD operations on subscriptions, including:
    /// - Fetching subscriptions with pagination and filtering.
    /// - Retrieving a specific subscription by ID.
    /// - Creating, updating, and deleting subscriptions.
    /// </remarks>
    /// <remarks>
    /// Initializes a new instance of the <see cref="SubscriptionsController"/> class.
    /// </remarks>
    /// <param name="service">Service for subscription operations.</param>
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController(ISubscriptionService service) : ControllerBase
    {
        private readonly ISubscriptionService _service = service;

        /// <summary>
        /// Retrieves a paginated list of subscriptions with optional search and filtering.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve (default is 1).</param>
        /// <param name="pageSize">The number of items per page (default is 10).</param>
        /// <param name="searchTerm">Optional search term to filter subscriptions.</param>
        /// <returns>A paginated list of subscriptions.</returns>
        /// <response code="200">Returns the paginated list of subscriptions.</response>
        /// <response code="401">If request is unauthorized.</response>
        /// <response code="404">If no subscriptions are found.</response>
        /// <response code="500">If an unexpected error occurred.</response>
        [Authorize(Roles = "ADMIN, MODERATOR")]
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<SubscriptionDto>>> GetAll(
            [FromQuery] int pageNumber = GlobalConstants.DefaultPageNumber,
            [FromQuery] int pageSize = GlobalConstants.DefaultPageSize,
            [FromQuery] string? searchTerm = null
        )
        {
            var subscriptions = await _service.GetAllAsync(pageNumber, pageSize, searchTerm);
            return Ok(subscriptions);
        }

        /// <summary>
        /// Retrieves a subscription by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the subscription.</param>
        /// <returns>The subscription with the specified ID.</returns>
        /// <response code="200">Returns the subscription if found.</response>
        /// <response code="401">If request is unauthorized.</response>
        /// <response code="404">If the subscription with the specified ID is not found or ID was not specified.</response>
        /// <response code="500">If an unexpected error occurred.</response>
        [Authorize(Roles = "ADMIN, MODERATOR")]
        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriptionDto>> GetById(Guid id)
        {
            if (id.Equals(Guid.Empty))
                return NotFound($"User ID [{id}] was not provided.");

            var subscription = await _service.GetByIdAsync(id);
            return Ok(subscription);
        }

        /// <summary>
        /// Creates a new subscription.
        /// </summary>
        /// <param name="subscription">The subscription data transfer object (DTO) containing subscription information.</param>
        /// <returns>The newly created subscription with its ID.</returns>
        /// <response code="201">Returns the newly created subscription.</response>
        /// <response code="400">If the provided subscription data is invalid.</response>
        /// <response code="401">If request is unauthorized.</response>
        /// <response code="500">If an unexpected error occurred.</response>
        [Authorize(Roles = "ADMIN, MODERATOR")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SubscriptionDto subscription)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.CreateAsync(subscription);
            return CreatedAtAction(nameof(GetById), new { id = subscription.Id }, subscription);
        }

        /// <summary>
        /// Updates an existing subscription.
        /// </summary>
        /// <param name="id">The ID of the subscription to update.</param>
        /// <param name="subscription">The updated subscription data.</param>
        /// <returns>No content if the update is successful.</returns>
        /// <response code="204">If the subscription is successfully updated.</response>
        /// <response code="400">If the subscription ID in the URL does not match the ID in the request body,
        /// or if the input is invalid, or subscription's data violates a business rule.</response>
        /// <response code="401">If request is unauthorized.</response>
        /// <response code="404">If the subscription to be updated does not exist.</response>
        /// <response code="500">If an unexpected error occurred.</response>
        [Authorize(Roles = "ADMIN, MODERATOR")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] SubscriptionDto subscription)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != subscription.Id)
                return BadRequest("Subscription ID in the URL does not match the ID in the body.");

            await _service.UpdateAsync(subscription);
            return NoContent();
        }

        /// <summary>
        /// Deletes a subscription by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the subscription to delete.</param>
        /// <returns>No content if the deletion is successful.</returns>
        /// <response code="204">If the subscription is successfully deleted.</response>
        /// <response code="401">If request is unauthorized.</response>
        /// <response code="404">If the subscription to be deleted does not exist.</response>
        /// <response code="500">If an unexpected error occurred.</response>
        [Authorize(Roles = "ADMIN, MODERATOR")]
        [HttpDelete("{id}")]
        public async Task<NoContentResult> DeleteSubscription(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Subscribes a user to a subscription.
        /// </summary>
        /// <param name="request">The request containing the user ID and subscription ID.</param>
        /// <returns>A confirmation that the user was successfully subscribed.</returns>
        /// <response code="200">The user was successfully subscribed.</response>
        /// <response code="400">The request data is invalid.</response>
        /// <response code="401">The request is unauthorized.</response>
        /// <response code="500">An unexpected server error occurred.</response>
        // [Authorize(Roles = "USER")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.SubscribeAsync(request);
            return Ok();
        }

        /// <summary>
        /// Unsubscribes a user from a subscription.
        /// </summary>
        /// <param name="request">The request containing the user ID and subscription ID.</param>
        /// <returns>A confirmation that the user was successfully unsubscribed.</returns>
        /// <response code="200">The user was successfully unsubscribed.</response>
        /// <response code="400">The request data is invalid.</response>
        /// <response code="401">The request is unauthorized.</response>
        /// <response code="500">An unexpected server error occurred.</response>
        // [Authorize(Roles = "USER")]
        [HttpPost("[action]")]
        public async Task<IActionResult> Unsubscribe([FromBody] SubscribeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.UnsubscribeAsync(request);
            return Ok();
        }
    }
}