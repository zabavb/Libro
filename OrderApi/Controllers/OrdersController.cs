using Microsoft.AspNetCore.Mvc;
using OrderApi.Services;

namespace OrderApi.Controllers
{
    /// <summary>
    /// Manage order-related operations
    /// </summary>
    /// <remarks>
    /// This controller provides CRUD operations for Orders
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IOrderService orderService, ILogger<OrdersController> logger) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;
        private readonly ILogger<OrdersController> _logger = logger;
        private string _message = string.Empty;

        /// <summary>
        /// Retrieves a paginated list of orders with optional search and filtering.
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve (default is 1).</param>
        /// <param name="pageSize">The number of items per page (default is 10).</param>
        /// <param name="searchTerm">Optional search term to filter orders.</param>
        /// <param name="filter">Optional filter criteria for orders.</param>
        /// <returns>A paginated list of orders.</returns>
        /// <response code="200">Returns the paginated list of orders.</response>
        /// <response code="500">an unexpected error occured.</response>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null, [FromQuery] Filter? filter = null, [FromQuery] Sort? sort = null)
        {
            try
            {
                var orders = await _orderService.GetAllAsync(pageNumber, pageSize, searchTerm!, filter, sort);
                _logger.LogInformation("Orders successfully fetched.");
                return Ok(orders);
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }

        }

        /// <summary>
        /// Retrieves Order by id
        /// </summary>
        /// <param name="id">Order id</param>
        /// <returns>Order which id matches with given one</returns>
        /// <response code="200">Retrieval successful, return the order</response>
        /// <response code="404">Could not find the order</response>
        /// <response code="500">an unexpected error occured.</response>
        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var order = await _orderService.GetByIdAsync(id);
                if (id.Equals(Guid.Empty))
                {
                    _message = $"Order ID {id} was not provided.";
                    _logger.LogError(_message);
                    return NotFound(new { message = _message });
                }

                return Ok(order);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status404NotFound, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves Order data for user's card for users' list page by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>Snippet of order data which ID matches with provided one in parameters.</returns>
        /// <response code="200">Retrieval successful, return the order snippet.</response>
        /// <response code="404">Could not find the order.</response>
        /// <response code="500">An unexpected error occured.</response>
        // [Authorize(Roles = "ADMIN, MODERATOR")]
        [HttpGet("for-user/card/{id}")]
        public async Task<ActionResult<OrderForUserCard>> GetForUserCardAsync(Guid id)
        {
            if (id == Guid.Empty)
                return NotFound($"User ID [{id}] was not provided.");

            var user = await _orderService.GetForUserCardAsync(id);
            return Ok(user);
        }

        /// <summary>
        /// Retrieves Orders data for user's details page by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>Snippets of order data which user ID matches with provided one in parameters.</returns>
        /// <response code="200">Retrieval successful, return the order snippets.</response>
        /// <response code="404">Could not find the user.</response>
        /// <response code="500">An unexpected error occured.</response>
        // [Authorize(Roles = "ADMIN, MODERATOR")]
        [HttpGet("for-user/details/{id}")]
        public async Task<ActionResult<ICollection<OrderForUserDetails>>> GetAllForUserDetailsAsync(Guid id)
        {
            if (id == Guid.Empty)
                return NotFound($"User ID [{id}] was not provided.");

            var snippets = await _orderService.GetAllForUserDetailsAsync(id);
            return Ok(snippets);
        }

        /// <summary>
        /// Creates a new order
        /// </summary>
        /// <param name="orderDto">Order data</param>
        /// <returns>Created order</returns>
        /// <response code="201">Order created successfully.</response>
        /// <response code="400">Invalid input data.</response>
        /// <response code="500"> an unexpected error occured.</response>
        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create([FromBody]OrderDto orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _orderService.CreateAsync(orderDto);
                _logger.LogInformation($"Order with Id [{orderDto.Id}] successfully created.");
                return CreatedAtAction(nameof(GetById), new { id = orderDto.Id }, orderDto);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Updates existing order
        /// </summary>
        /// <param name="id">The ID of the order to update.</param>
        /// <param name="orderDto">Updated order data</param>
        /// <returns>The updated order</returns>
        /// <response code="204">the order is successfully updated.</response>
        /// <response code="400">the order ID in the URL does not match the ID in the request body, or if the input is invalid.</response>
        /// <response code="404">the order to be updated does not exist.</response>
        /// <response code="500">an unexpected error occured.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] OrderDto orderDto)
        {
            if (orderDto != null && id != orderDto.Id)
            {
                _message = "Order ID in the URL does not match the ID in the body.";
                _logger.LogError(_message);
                return BadRequest(_message);
            }

            try
            {
                await _orderService.UpdateAsync(orderDto!);
                _logger.LogInformation($"Order with Id [{orderDto!.Id}] successfully updated.");
                return NoContent();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ModelState);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a order by id
        /// </summary>
        /// <param name="id">Order id</param>
        /// <returns>NoContent on success</returns>
        /// <response code="204">Order deleted successfully.</response>
        /// <response code="404">Could not find the order.</response>
        /// <response code="500">an unexpected error occured.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _orderService.DeleteAsync(id);
                _logger.LogInformation($"Order with Id [{id}] successfully deleted.");
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
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
