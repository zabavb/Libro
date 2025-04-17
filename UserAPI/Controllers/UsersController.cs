using BookAPI;
using Library.Common;
using Library.DTOs.UserRelated.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAPI.Services.Interfaces;

namespace UserAPI.Controllers
{
    /// <summary>
    /// Controller for managing user-related operations.
    /// </summary>
    /// <remarks>
    /// This controller provides endpoints for performing CRUD operations on users, including:
    /// - Fetching users with pagination and filtering.
    /// - Retrieving a specific user by ID.
    /// - CRUD (Creating, updating, and deleting) users.
    /// </remarks>
    /// <remarks>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </remarks>
    /// <param name="service">Service for user operations.</param>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService service) : ControllerBase
    {
        private readonly IUserService _service = service;

        /// <summary>
        /// Retrieves a paginated list of users with optional search and filtering.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve (default is 1).</param>
        /// <param name="pageSize">The number of items per page (default is 10).</param>
        /// <param name="searchTerm">Optional search term to filter users.</param>
        /// <param name="filter">Optional filter criteria for users.</param>
        /// <param name="sort">Optional sort criteria for users.</param>
        /// <returns>A paginated list of users.</returns>
        /// <response code="200">Returns the paginated list of users.</response>
        /// <response code="401">If request is unauthorized.</response>
        /// <response code="404">If no users are found.</response>
        /// <response code="500">If an unexpected error occurred.</response>
        // [Authorize(Roles = "ADMIN, MODERATOR")]
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<Dto>>> GetAll(
            [FromQuery] int pageNumber = GlobalConstants.DefaultPageNumber,
            [FromQuery] int pageSize = GlobalConstants.DefaultPageSize,
            [FromQuery] string? searchTerm = null,
            [FromQuery] Filter? filter = null,
            [FromQuery] Sort? sort = null
        )
        {
            var users = await _service.GetAllAsync(pageNumber, pageSize, searchTerm, filter, sort);
            return Ok(users);
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>The user with the specified ID.</returns>
        /// <response code="200">Returns the user if found.</response>
        /// <response code="401">If request is unauthorized.</response>
        /// <response code="404">If the user with the specified ID is not found or ID was not specified.</response>
        /// <response code="500">If an unexpected error occurred.</response>
        // [Authorize(Roles = "ADMIN, MODERATOR")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserWithSubscriptionsDto>> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return NotFound($"User ID [{id}] was not provided.");

            var user = await _service.GetByIdAsync(id);
            return Ok(user);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">The user data transfer object (DTO) containing user information.</param>
        /// <returns>The newly created user with its ID.</returns>
        /// <response code="201">Returns the newly created user.</response>
        /// <response code="400">If the provided user data is invalid or violates a business rule.</response>
        /// <response code="500">If an unexpected error occurred.</response>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Dto user)
        {
            if (ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.CreateAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="user">The updated user data.</param>
        /// <returns>No content if the update is successful.</returns>
        /// <response code="204">If the user is successfully updated.</response>
        /// <response code="400">If the user ID in the URL does not match the ID in the request body,
        /// or if the input is invalid, or user's data violates a business rule.</response>
        /// <response code="401">If request is unauthorized.</response>
        /// <response code="404">If the user to be updated does not exist.</response>
        /// <response code="500">If an unexpected error occurred.</response>
        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Dto user)
        {
            if (ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != user.Id)
                return BadRequest("User ID in the URL does not match the ID in the body.");

            await _service.UpdateAsync(user);
            return NoContent();
        }

        /// <summary>
        /// Deletes a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>No content if the deletion is successful.</returns>
        /// <response code="204">If the user is successfully deleted.</response>
        /// <response code="401">If request is unauthorized.</response>
        /// <response code="404">If the user to be deleted does not exist.</response>
        /// <response code="500">If an unexpected error occurred.</response>
        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public async Task<NoContentResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}