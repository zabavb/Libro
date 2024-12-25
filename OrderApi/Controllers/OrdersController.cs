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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;
        private string _message;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdersController"/> class.
        /// </summary>
        /// <param name="orderService">Service for order operations.</param>
        /// <param name="logger">Logger for tracking operations.</param>
        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
            _message = string.Empty;
        }

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
        public async Task<IActionResult> GetOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null, [FromQuery] Filter? filter = null)
        {
            try
            {
                var orders = await _orderService.GetOrdersAsync(pageNumber, pageSize, searchTerm!, filter);
                return Ok(orders);
            }
            catch (Exception ex) {
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
        [HttpGet("{id}")]

        public async Task<ActionResult<OrderDto>> GetOrderById(Guid id)
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
        /// Creates a new order
        /// </summary>
        /// <param name="orderDto">Order data</param>
        /// <returns>Created order</returns>
        /// <response code="201">Order created successfully.</response>
        /// <response code="400">Invalid input data.</response>
        /// <response code="500">Object with the given Id already exists.</response>
        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody]OrderDto orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _orderService.AddAsync(orderDto);
                _logger.LogInformation($"Order with Id [{orderDto.Id}] successfully created.");
                return CreatedAtAction(nameof(GetOrderById), new { id = orderDto.Id }, orderDto);
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
        /// <param name="orderDto">Updated order data</param>
        /// <returns>The updated order</returns>
        /// <response code="204">the order is successfully updated.</response>
        /// <response code="400">the order ID in the URL does not match the ID in the request body, or if the input is invalid.</response>
        /// <response code="404">the order to be updated does not exist.</response>
        /// <response code="500">an unexpected error occured.</response>
        [HttpPut]
        public async Task<IActionResult> UpdateOrder([FromBody]OrderDto orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _orderService.UpdateAsync(orderDto);
                _logger.LogInformation($"Delivery type with Id [{orderDto.Id}] successfully updated.");
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
        public async Task<IActionResult> DeleteOrder(Guid id)
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
