using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shadow_Tech.Data;
using Shadow_Tech.Models;
using System;

namespace Shadow_Tech.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public CategoriesController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            // Interogare pentru categorii și numărul de produse asociate
            var categoriesWithProductCount = db.Categories
                .Select(category => new
                {
                    category.Id,
                    category.Name,
                    ProductCount = category.Products.Count(product => product.Listed) // Numărul de produse asociate
                })
                .OrderBy(c => c.Name)
                .ToList();

            // Transmiterea datelor către view prin ViewBag
            ViewBag.Categories = categoriesWithProductCount;

            return View();
        }

        public IActionResult New()
        {
            

            return View();
        }
    }
}
