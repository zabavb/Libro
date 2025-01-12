using Microsoft.AspNetCore.Mvc;
using UserAPI.Models;
using UserAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Library.Extensions;

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        // GET: api/Subscription
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<SubscriptionDto>>> GetAllSubscriptions(int pageNumber, int pageSize, string searchTerm = "")
        {
            var subscriptions = await _subscriptionService.GetAllAsync(pageNumber, pageSize, searchTerm);
            return Ok(subscriptions);
        }

        // GET: api/Subscription/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriptionDto>> GetSubscriptionById(Guid id)
        {
            var subscription = await _subscriptionService.GetByIdAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }
            return Ok(subscription);
        }

        // POST: api/Subscription
        [HttpPost]
        public async Task<IActionResult> CreateSubscription([FromBody] SubscriptionDto subscriptionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _subscriptionService.CreateAsync(subscriptionDto);
            return CreatedAtAction(nameof(GetSubscriptionById), new { id = subscriptionDto.Id }, subscriptionDto);
        }

        // PUT: api/Subscription/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubscription(Guid id, [FromBody] SubscriptionDto subscriptionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != subscriptionDto.Id)
            {
                return BadRequest("ID mismatch.");
            }
            await _subscriptionService.UpdateAsync(subscriptionDto);
            return NoContent();
        }

        // DELETE: api/Subscription/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscription(Guid id)
        {
            await _subscriptionService.DeleteAsync(id);
            return NoContent();
        }
    }
}
