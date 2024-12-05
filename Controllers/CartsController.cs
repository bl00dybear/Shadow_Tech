using Microsoft.AspNetCore.Mvc;

namespace Shadow_Tech.Controllers
{
    public class CartsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
