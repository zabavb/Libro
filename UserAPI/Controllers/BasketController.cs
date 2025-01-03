/*using Microsoft.AspNetCore.Mvc;
using UserAPI.Services;

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : Controller
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetBasket()
        {
            var basket = _basketService.GetBasket(HttpContext);
            return Ok(basket);
        }

        [HttpPost("add")]
        public IActionResult AddItem([FromBody] BasketItemDto item)
        {
            _basketService.AddItem(HttpContext, item);
            return Ok("Item added to basket.");
        }

        [HttpPut("update")]
        public IActionResult UpdateItem([FromQuery] Guid productId, [FromQuery] int quantity)
        {
            _basketService.UpdateQuantity(HttpContext, productId, quantity);
            return Ok("Item quantity updated.");
        }

        [HttpDelete("remove/{productId}")]
        public IActionResult RemoveItem(Guid productId)
        {
            _basketService.RemoveItem(HttpContext, productId);
            return Ok("Item removed from basket.");
        }

        [HttpDelete("clear")]
        public IActionResult ClearBasket()
        {
            _basketService.ClearBasket(HttpContext);
            return Ok("Basket cleared.");
        }
    }
}
*/