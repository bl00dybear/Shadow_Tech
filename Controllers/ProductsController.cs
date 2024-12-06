using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shadow_Tech.Data;
using Shadow_Tech.Models;
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

        public IActionResult New()
        {
            Product product = new Product();
            product.Categ = GetAllCategories();

            return View(product);
        }
       
        
        

        [NonAction]
    public IEnumerable<SelectListItem> GetAllCategories()
    {
        // generam o lista de tipul SelectListItem fara elemente
        var selectList = new List<SelectListItem>();

        // extragem toate categoriile din baza de date
        var categories = from cat in db.Categories
                         select cat;

        // iteram prin categorii
        foreach (var category in categories)
        {
            // adaugam in lista elementele necesare pentru dropdown
            // id-ul categoriei si denumirea acesteia
            selectList.Add(new SelectListItem
            {
                Value = category.Id.ToString(),
                Text = category.Name
            });
        }
        /* Sau se poate implementa astfel: 
         * 
        foreach (var category in categories)
        {
            var listItem = new SelectListItem();
            listItem.Value = category.Id.ToString();
            listItem.Text = category.CategoryName;

            selectList.Add(listItem);
         }*/


        // returnam lista de categorii
        return selectList;
    }
    }
}
