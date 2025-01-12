using Microsoft.AspNetCore.Mvc;
using UserAPI.Services;

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordController : Controller
    {
        private readonly IPasswordService _passwordService;
        
        public PasswordController(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                var password = await _passwordService.GetByIdAsync(id);
                return Ok(password);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });

            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] UserDto user, string password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _passwordService.AddAsync(password, user);
                
                return CreatedAtAction(nameof(GetByIdAsync), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] UserDto user, string newPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _passwordService.UpdateAsync(user, newPassword);
                return NoContent();
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePassword(Guid id)
        {
            try
            {
                await _passwordService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
