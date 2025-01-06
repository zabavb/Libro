using Microsoft.AspNetCore.Mvc;
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
    /// <remarks>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </remarks>
        /// <param name="userService">Service for user operations.</param>
        /// <param name="logger">Logger for tracking operations.</param>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserService userService, ILogger<UsersController> logger) : ControllerBase
        {
        private readonly IUserService _userService = userService;
        private readonly ILogger<UsersController> _logger = logger;
        private string _message = string.Empty;

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
        /// <response code="500">If an unexpected error occurs.</response>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null, [FromQuery] Filter? filter = null, [FromQuery] Sort? sort = null)
        {
            try
            {
                var users = await _userService.GetAllAsync(pageNumber, pageSize, searchTerm, filter, sort);
                _logger.LogInformation("Users successfully fetched.");
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>The user with the specified ID.</returns>
        /// <response code="200">Returns the user if found.</response>
        /// <response code="404">If the user with the specified ID is not found or ID was not specified.</response>
        /// <response code="500">If an unexpected error occurs.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (id.Equals(Guid.Empty))
                {
                    _message = $"User ID [{id}] was not provided.";
                    _logger.LogError(_message);
                    return NotFound(_message);
                }

                _logger.LogInformation($"User with ID [{id}] successfully fetched.");
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
        public async Task<IActionResult> Create([FromBody] UserDto user)
        {
            try
            {
                await _userService.CreateAsync(user);
                _logger.LogInformation($"User with ID [{user.Id}] successfully created.");
                return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
        public async Task<IActionResult> Update(Guid id, [FromBody] UserDto user)
        {
            if (user != null && id != user.Id)
            {
                _message = "User ID in the URL does not match the ID in the body.";
                _logger.LogError(_message);
                return BadRequest(_message);
            }

            try
            {
                await _userService.UpdateAsync(user!);
                _logger.LogInformation($"User with ID [{user!.Id}] successfully updated.");
                return NoContent();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
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
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _userService.DeleteAsync(id);
                _logger.LogInformation($"User with ID [{id}] successfully deleted.");
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
