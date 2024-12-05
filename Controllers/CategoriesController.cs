using Microsoft.AspNetCore.Mvc;

namespace Shadow_Tech.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
