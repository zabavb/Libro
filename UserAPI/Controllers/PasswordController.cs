using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "ADMIN, MODERATOR, USER")]
        [HttpPut("{userId}")]
        public async Task<IActionResult> Update(Guid userId, [FromBody] string newPassword)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException($"New password for user ID [{userId}] was not provided.",
                    nameof(newPassword));
            try
            {
                await _passwordService.UpdateAsync(userId, newPassword);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }
    }
}