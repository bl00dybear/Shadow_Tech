using Microsoft.AspNetCore.Mvc;
using Shadow_Tech.Data;
using System;

namespace Shadow_Tech.Controllers
{
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext db;
        public CartsController(ApplicationDbContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
