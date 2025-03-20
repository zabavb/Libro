using Microsoft.AspNetCore.Mvc;
using UserAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using Library.Extensions;
using UserAPI.Services.Interfaces;

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
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionController"/> class.
        /// </summary>
        /// <param name="subscriptionService">Service for subscription operations.</param>
        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        /// <summary>
        /// Retrieves a paginated list of subscriptions with optional search and filtering.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="searchTerm">Optional search term to filter subscriptions.</param>
        /// <returns>A paginated list of subscriptions.</returns>
        /// <response code="200">Returns the paginated list of subscriptions.</response>
        /// <response code="500">If an unexpected error occurs.</response>
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<SubscriptionDto>>> GetAllSubscriptions(int pageNumber, int pageSize, string searchTerm = "")
        {
            try
            {
                var subscriptions = await _subscriptionService.GetAllAsync(pageNumber, pageSize, searchTerm);
                return Ok(subscriptions);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a subscription by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the subscription.</param>
        /// <returns>The subscription with the specified ID.</returns>
        /// <response code="200">Returns the subscription if found.</response>
        /// <response code="404">If the subscription with the specified ID is not found.</response>
        /// <response code="500">If an unexpected error occurs.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriptionDto>> GetSubscriptionById(Guid id)
        {
            try
            {
                var subscription = await _subscriptionService.GetByIdAsync(id);
                if (subscription == null)
                    return NotFound();

                return Ok(subscription);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Creates a new subscription.
        /// </summary>
        /// <param name="subscriptionDto">The subscription data transfer object (DTO) containing subscription information.</param>
        /// <returns>The newly created subscription with its ID.</returns>
        /// <response code="201">Returns the newly created subscription.</response>
        /// <response code="400">If the provided subscription data is invalid.</response>
        /// <response code="500">If an unexpected error occurs.</response>
        [HttpPost]
        public async Task<IActionResult> CreateSubscription([FromBody] SubscriptionDto subscriptionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _subscriptionService.CreateAsync(subscriptionDto);
                return CreatedAtAction(nameof(GetSubscriptionById), new { id = subscriptionDto.Id }, subscriptionDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing subscription.
        /// </summary>
        /// <param name="id">The ID of the subscription to update.</param>
        /// <param name="subscriptionDto">The updated subscription data.</param>
        /// <returns>No content if the update is successful.</returns>
        /// <response code="204">If the subscription is successfully updated.</response>
        /// <response code="400">If the subscription ID in the URL does not match the ID in the request body, or if the input is invalid.</response>
        /// <response code="404">If the subscription to be updated does not exist.</response>
        /// <response code="500">If an unexpected error occurs.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubscription(Guid id, [FromBody] SubscriptionDto subscriptionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != subscriptionDto.Id)
                return BadRequest("ID mismatch.");

            try
            {
                await _subscriptionService.UpdateAsync(subscriptionDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Deletes a subscription by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the subscription to delete.</param>
        /// <returns>No content if the deletion is successful.</returns>
        /// <response code="204">If the subscription is successfully deleted.</response>
        /// <response code="404">If the subscription to be deleted does not exist.</response>
        /// <response code="500">If an unexpected error occurs.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscription(Guid id)
        {
            try
            {
                await _subscriptionService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
