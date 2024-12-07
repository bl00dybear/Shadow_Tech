using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Shadow_Tech.Data;
using Shadow_Tech.Models;
using System;

namespace Shadow_Tech.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ProductsController(
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
        [HttpPost]
        public IActionResult New(Product product)
        {
            if (ModelState.IsValid)
            {

                db.Products.Add(product);
                db.SaveChanges();
                TempData["message"] = "Produsul a fost adaugat";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");
            }
            else
            {
                product.Categ = GetAllCategories();
                return View(product);
            }
        }

        public IActionResult Show(int id)
        {
            var product = db.Products
                .Include(p => p.Category)
                .Include(p => p.Reviews)
                .Include(r => r.User)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [HttpPost]
        public IActionResult Show(int id, int Rating, string Comment)
        {
            // Căutăm produsul asociat
            var product = db.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            // Creăm un review nou
            var review = new Review
            {
                ProductId = id,
                Rating = Rating,
                Comment = Comment
            };

            // Adăugăm recenzia în baza de date
            db.Reviews.Add(review);
            db.SaveChanges();

            // Adăugăm un mesaj pentru utilizator
            TempData["message"] = "Recenzia a fost adăugată cu succes!";
            TempData["messageType"] = "alert-success";

            // Redirecționăm înapoi la pagina Show
            return RedirectToAction("Show", new { id });
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

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            //Console.WriteLine("Am adaugat produsul cu id-ul: " + i);
            var product = db.Products.FirstOrDefault(p => p.Id == productId);

            if (product != null)
            {
                var item = new Cart
                {
                    ProductId = product.Id,
                    ProductName = product.Title,
                    Price = product.Price,
                    UserId = 1,
                    Quantity = 1,
                    Photo = product.Photo
                };

                db.Carts.Add(item);
                db.SaveChanges();

                return Redirect("Show/"+product.Id);
            }
            else
            {
                return NotFound();
            }

            
        }
    
    }
}
