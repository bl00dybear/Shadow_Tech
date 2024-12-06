using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shadow_Tech.Data;
using System;

namespace Shadow_Tech.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext db;
        public ProductsController(ApplicationDbContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            var products = db.Products.Include("Category").ToList();
            //ViewBag.Products = products;

            return View(products);
        }
    }
}
