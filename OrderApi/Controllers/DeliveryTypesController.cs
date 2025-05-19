using Microsoft.AspNetCore.Mvc;
using OrderAPI;
using OrderAPI.Services.Interfaces;
namespace OrderApi.Controllers
{
    /// <summary>
    /// Manage delivery type related operations
    /// </summary>
    /// <remarks>
    /// This controller provides CRUD operations for Delivery types
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryTypesController(IDeliveryTypeService deliveryTypeService, ILogger<DeliveryTypesController> logger) : ControllerBase
    {
        private readonly IDeliveryTypeService _deliveryTypeService = deliveryTypeService;
        private readonly ILogger<DeliveryTypesController> _logger = logger;
        private string _message = string.Empty;

        /// <summary>
        /// Retrieves list of delivery type
        /// </summary>
        /// <param name="pageNumber">The page number to retrieve (default is 1).</param>
        /// <param name="pageSize">The number of items per page (default is 10).</param>
        /// <param name="searchTerm">Optional search term to filter orders.</param>
        /// <returns>List of delivery types</returns>
        /// <response code="200">Retrieval successful, returns the list</response>
        /// <response code="500">an unexpected error occured.</response>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null,[FromQuery] DeliverySort? sort = null)
        {
            try
            {
                var deliveryTypes = await _deliveryTypeService.GetAllAsync(pageNumber, pageSize, searchTerm!, sort);
                _logger.LogInformation("Delivery types succesfully fetched.");
                return Ok(deliveryTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves Delivery type by id
        /// </summary>
        /// <param name="id">Delivery type id</param>
        /// <returns>Delivery type which id matches with given one</returns>
        /// <response code="200">Retrieval successful, return the delivery type.</response>
        /// <response code="404">Could not find the delivery type.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryTypeDto>> GetById(Guid id)
        {
            try
            {
                var deliveryType = await _deliveryTypeService.GetByIdAsync(id);
                if (id.Equals(Guid.Empty))
                {
                    _message = $"Delivery type ID {id} was not provided.";
                    _logger.LogError(_message);
                    return NotFound(new { message = _message });
                }

                return Ok(deliveryType);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status404NotFound, new { message = ex.Message });
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Creates a new delivery type
        /// </summary>
        /// <param name="deliveryTypeDto">Delivery type data</param>
        /// <returns>Created delivery type</returns>
        /// <response code="201">Delivery type created successfully.</response>
        /// <response code="400">Invalid input data.</response>
        /// <response code="500">Object with the given Id already exists.</response>
        [HttpPost]
        public async Task<ActionResult<DeliveryTypeDto>> Create([FromBody] DeliveryTypeDto deliveryTypeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _deliveryTypeService.CreateAsync(deliveryTypeDto);
                _logger.LogInformation($"Delivery types with Id [{deliveryTypeDto.Id}] successfully created.");
                return CreatedAtAction(nameof(GetById), new { id = deliveryTypeDto.Id }, deliveryTypeDto);
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
        /// Updates existing deliveryType
        /// </summary>
        /// <param name="id">UThe ID of the delivery type to update. </param>
        /// <param name="deliveryTypeDto">Updated delivery type data. </param>
        /// <returns>The updated delivery type</returns>
        /// <response code="204">the delivery type is successfully updated.</response>
        /// <response code="400">the delivery type ID in the URL does not match the ID in the request body, or if the input is invalid.</response>
        /// <response code="404">the delivery type to be updated does not exist.</response>
        /// <response code="500">an unexpected error occured.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] DeliveryTypeDto deliveryTypeDto)
        {
            if (deliveryTypeDto != null && id != deliveryTypeDto.Id)
            {
                _message = "Delivery Type ID in the URL does not match the ID in the body.";
                _logger.LogError(_message);
                return BadRequest(_message);
            }

            try
            {
                await _deliveryTypeService.UpdateAsync(deliveryTypeDto!);
                _logger.LogInformation($"Delivery type with Id [{deliveryTypeDto!.Id}] successfully updated.");
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
        /// Deletes a delivery type by id
        /// </summary>
        /// <param name="id">Delivery type id</param>
        /// <returns>NoContent on success</returns>
        /// <response code="204">Delivery type deleted successfully.</response>
        /// <response code="404">Could not find the delivery type.</response>
        /// <response code="500">an unexpected error occured.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _deliveryTypeService.DeleteAsync(id);
                _logger.LogInformation($"Delivery types with Id [{id}] successfully deleted.");
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(new { message = ex.Message} );
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
