﻿using Microsoft.AspNetCore.Mvc;
using UserAPI.Models.Extensions;
using UserAPI.Services;

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
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;
        private string _message;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService">Service for user operations.</param>
        /// <param name="logger">Logger for tracking operations.</param>
        /// <param name="message">Message container for log messages.</param>
        public UsersController(IUserService userService, ILogger<UsersController> logger, string message)
        {
            _userService = userService;
            _logger = logger;
            _message = message;
        }

        /// <summary>
        /// Retrieves a paginated list of users with optional search and filtering.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve (default is 1).</param>
        /// <param name="pageSize">The number of items per page (default is 10).</param>
        /// <param name="searchTerm">Optional search term to filter users.</param>
        /// <param name="filter">Optional filter criteria for users.</param>
        /// <returns>A paginated list of users.</returns>
        /// <response code="200">Returns the paginated list of users.</response>
        /// <response code="500">If an unexpected error occurs.</response>
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null, [FromQuery] UserFilter? filter = null)
        {
            try
            {
                var users = await _userService.GetAllEntitiesPaginatedAsync(pageNumber, pageSize, searchTerm!, filter);
                _logger.LogInformation("Users successfully fetched.");
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>The user with the specified ID.</returns>
        /// <response code="200">Returns the user if found.</response>
        /// <response code="404">If the user with the specified ID is not found.</response>
        /// <response code="500">If an unexpected error occurs.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var user = await _userService.GetEntityByIdAsync(id);
                if (user == null)
                {
                    _message = $"User with ID {id} was not provided.";
                    _logger.LogError(_message);
                    return NotFound(new { message = _message });
                }

                _logger.LogInformation($"User with ID [{id}] successfully fetched.");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">The user data transfer object (DTO) containing user information.</param>
        /// <returns>The newly created user with its ID.</returns>
        /// <response code="201">Returns the newly created user.</response>
        /// <response code="400">If the provided user data is invalid.</response>
        /// <response code="500">If an unexpected error occurs.</response>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDto user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _userService.AddEntityAsync(user);
                _logger.LogInformation($"User with ID [{user.Id}] successfully created.");
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="user">The updated user data.</param>
        /// <returns>No content if the update is successful.</returns>
        /// <response code="204">If the user is successfully updated.</response>
        /// <response code="400">If the user ID in the URL does not match the ID in the request body, or if the input is invalid.</response>
        /// <response code="404">If the user to be updated does not exist.</response>
        /// <response code="500">If an unexpected error occurs.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserDto user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != user.Id)
            {
                _message = "User ID in the URL does not match the ID in the body.";
                _logger.LogError(_message);
                return BadRequest(new { message = _message });
            }

            try
            {
                await _userService.UpdateEntityAsync(user);
                _logger.LogInformation($"User with ID [{user.Id}] successfully updated.");
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>No content if the deletion is successful.</returns>
        /// <response code="204">If the user is successfully deleted.</response>
        /// <response code="404">If the user to be deleted does not exist.</response>
        /// <response code="500">If an unexpected error occurs.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                await _userService.DeleteEntityAsync(id);
                _logger.LogInformation($"User with ID [{id}] successfully deleted.");
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}