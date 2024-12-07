using Microsoft.AspNetCore.Mvc;
using Shadow_Tech.Data;
using System;

namespace Shadow_Tech.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext db;
        public CategoriesController(ApplicationDbContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
