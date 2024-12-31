using Library.DTOs.Order;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Services;

namespace OrderApi.Controllers
{
    /// <summary>
    /// Manage order related operations
    /// </summary>
    /// <remarks>
    /// This controller provides CRUD operations for Orders
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<StatusController> _logger;
        private string _message;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusController"/> class.
        /// </summary>
        /// <param name="orderService">Service for order operations.</param>
        /// <param name="logger">Logger for tracking operations.</param>
        public StatusController(IOrderService orderService, ILogger<StatusController> logger)
        {
            _orderService = orderService;
            _logger = logger;
            _message = string.Empty;
        }

        /// <summary>
        /// Changes Order status in by id
        /// </summary>
        /// <param name="id">Updated order id</param>
        /// <param name="orderStatus">New status</param>
        /// <response code="204">the order is successfully updated.</response>
        /// <response code="400">the order ID in the URL does not match the ID in the request body, or if the input is invalid.</response>
        /// <response code="404">the order to be updated does not exist.</response>
        /// <response code="500">an unexpected error occured.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromBody] OrderStatus orderStatus)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var order = await _orderService.GetByIdAsync(id);
                order.Status = orderStatus;
                await _orderService.UpdateAsync(order);
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
    }
}
