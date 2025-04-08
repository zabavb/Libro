using Microsoft.AspNetCore.Mvc;
using UserAPI.Services.Interfaces;

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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] Dto user, string oldPassword, string newPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _passwordService.UpdateAsync(user, oldPassword, newPassword);
                return NoContent();
            }

            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}
