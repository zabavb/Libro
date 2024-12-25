using Microsoft.AspNetCore.Mvc;

namespace UserAPI.Controllers
{
    public class BasketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
