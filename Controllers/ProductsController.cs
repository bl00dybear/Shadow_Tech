using Microsoft.AspNetCore.Mvc;

namespace Shadow_Tech.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
