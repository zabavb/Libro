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
    public class StatusController(IOrderService orderService, ILogger<StatusController> logger) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;
        private readonly ILogger<StatusController> _logger = logger;
        /// <summary>
        /// Changes Order status in by id
        /// </summary>
        /// <param name="id">Updated order id</param>
        /// <param name="orderStatus">New status</param>
        /// <response code="204">the order is successfully updated.</response>
        /// <response code="400">the order ID in the URL does not match the ID in the request body, or if the input is invalid.</response>
        /// <response code="404">the order to be updated does not exist.</response>
        /// <response code="500">an unexpected error occurred.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] OrderStatus orderStatus)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var order = await _orderService.GetByIdAsync(id);
                order.Status = orderStatus;
                await _orderService.UpdateAsync(order);
                _logger.LogInformation($"Status of Order with ID [{id}] updated successfully.");
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
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
