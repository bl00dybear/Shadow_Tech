using Humanizer;
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
        [AllowAnonymous]
        public IActionResult Index(int? id)
        {


            var products = db.Products
                             .Include(p => p.Category)
                             .Where(prod => prod.Listed && (id == null || prod.CategoryId == id));

            var search = "";
            if (Convert.ToString(HttpContext.Request.Query["search"]) != null)
            {
                search = Convert.ToString(HttpContext.Request.Query["search"]).Trim();

                List<int> productsIds = db.Products
                    .Where(p => p.Title.Contains(search))
                    .Select(p => p.Id)
                    .ToList();

                products = db.Products.Where(product => productsIds.Contains(product.Id) && product.Listed);

            }

            ViewBag.SearchingString = search;




            SetAccessRights();


            return View(products.ToList());
        }


        [Authorize(Roles = "Contribuitor,Admin")]
        public IActionResult New()
        {
            Product product = new Product();
            product.Categ = GetAllCategories();

            return View(product);
        }
        [Authorize(Roles = "Contribuitor,Admin")]
        [HttpPost]
        public IActionResult New(Product product)
        {
            if (ModelState.IsValid)
            {
                if (User.IsInRole("Admin"))
                    product.Listed = true;
                else
                    product.Listed = false;

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
                .Include(r => r.User) // Include și utilizatorul asociat fiecărei recenzii
                .FirstOrDefault(p => p.Id == id);

            SetAccessRights();

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Reviews = product.Reviews ?? new List<Review>();
            return View(product);
        }

        [HttpPost]
        [Authorize] // Permite doar utilizatorilor autentificați să lase recenzii
        public IActionResult Show(int id, int Rating, string Comment)
        {
            var product = db.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var review = new Review
            {
                ProductId = id,
                Rating = Rating,
                Comment = Comment,
                UserId = _userManager.GetUserId(User), // Setează utilizatorul curent
                
            };

            db.Reviews.Add(review);
            db.SaveChanges();

            TempData["message"] = "Recenzia a fost adăugată cu succes!";
            TempData["messageType"] = "alert-success";

            return RedirectToAction("Show", new { id });
        }



        [Authorize(Roles = "Contribuitor,Admin")]
        public IActionResult Edit(int id)
        {

            Product product = db.Products.Include("Category")
                                         .Where(art => art.Id == id)
                                         .First();

            product.Categ = GetAllCategories();

            if ((product.UserId == _userManager.GetUserId(User)) ||
                User.IsInRole("Admin"))
            {
                return View(product);
            }
            else
            {

                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui produs care nu va apartine";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }
        }
        [Authorize(Roles = "Contribuitor,Admin")]
        [HttpPost]
        public IActionResult Edit(int id, Product requestProduct)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (product.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin")) {
                    product.Title = requestProduct.Title;
                    product.Description = requestProduct.Description;
                    product.Price = requestProduct.Price;
                    product.CategoryId = requestProduct.CategoryId;
                    product.Photo = requestProduct.Photo;
                    if (User.IsInRole("Admin"))
                        product.Listed = true;
                    else
                        product.Listed = false;
                    TempData["message"] = "Produsul a fost modificat";
                    TempData["messageType"] = "alert-success";
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui produs care nu va apartine";
                    TempData["messageType"] = "alert-danger";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                requestProduct.Categ = GetAllCategories();
                return View(requestProduct);
            }

        }
        [Authorize(Roles = "Contribuitor,Admin")]
        public IActionResult EditAdmin(int id)
        {

            Product product = db.Products.Include("Category")
                                         .Where(art => art.Id == id)
                                         .First();

            product.Categ = GetAllCategories();

            if ((product.UserId == _userManager.GetUserId(User)) ||
                User.IsInRole("Admin"))
            {
                return View(product);
            }
            else
            {

                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui produs care nu va apartine";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("ValidateProducts");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditAdmin(int id, Product requestProduct)
        {
            Product product = db.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                if (product.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
                {
                    product.Title = requestProduct.Title;
                    product.Description = requestProduct.Description;
                    product.Price = requestProduct.Price;
                    product.CategoryId = requestProduct.CategoryId;
                    product.Photo = requestProduct.Photo;
                    TempData["message"] = "Produsul a fost modificat";
                    TempData["messageType"] = "alert-success";
                    db.SaveChanges();
                    return RedirectToAction("ValidateProducts");
                }
                else
                {
                    TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui produs care nu va apartine";
                    TempData["messageType"] = "alert-danger";
                    return RedirectToAction("ValidateProducts");
                }
            }
            else
            {
                requestProduct.Categ = GetAllCategories();
                return View(requestProduct);
            }

        }
        [Authorize(Roles = "Contribuitor,Admin")]
        public IActionResult Delete(int id)
        {
            

            Product product = db.Products.Include("Category")
                                         .Where(prod => prod.Id == id)
                                         .First();

            if (User.IsInRole("Admin"))
            {
                db.Products.Remove(product);
                db.SaveChanges();
                TempData["message"] = "Produsul a fost sters";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti un produs care nu va apartine";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }
        }
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteAdmin(int id)
        {
            

            Product product = db.Products.Include("Category")
                                         .Where(prod => prod.Id == id)
                                         .First();

            if (User.IsInRole("Admin"))
            {
                db.Products.Remove(product);
                db.SaveChanges();
                TempData["message"] = "Produsul a fost sters";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("ValidateProducts");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti un produs care nu va apartine";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("ValidateProducts");
            }
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ValidateProducts()
        {
            var products = db.Products
                             .Include(p => p.Category)
                             .Where(prod => prod.Listed == false);


            ViewBag.Products = products;


            return View();

        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Validate(int id)
        {
            Product product = db.Products.Find(id);

            if (product != null)
            {
                product.Listed = true;

                db.SaveChanges();

                TempData["message"] = "Produsul a fost validat.";
                TempData["messageType"] = "alert-success";
            }
            else
            {
                TempData["message"] = "Produsul nu a fost găsit.";
                TempData["messageType"] = "alert-danger";
            }

            return RedirectToAction("ValidateProducts");
        }

        private void SetAccessRights()
        {
            ViewBag.AfisareButoane = false;

            if (User.IsInRole("Contribuitor"))
            {
                ViewBag.AfisareButoane = true;
            }

            ViewBag.UserCurent = _userManager.GetUserId(User);

            ViewBag.EsteAdmin = User.IsInRole("Admin");
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            var selectList = new List<SelectListItem>();

            var categories = from cat in db.Categories
                             select cat;

            foreach (var category in categories)
            {
 
                selectList.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name
                });
            }

            return selectList;
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var product = db.Products.FirstOrDefault(p => p.Id == productId);

            if (product != null)
            {
                var item = db.Carts.FirstOrDefault(i => i.ProductId == productId);

                if (item != null)
                {
                    item.Quantity++;
                    db.SaveChanges();
                    return Redirect("Show/" + product.Id);
                }
                else
                {
                    var cartItem = new Cart
                    {
                        ProductId = product.Id,
                        ProductName = product.Title,
                        Price = product.Price,
                        UserId = 1,
                        Quantity = 1,
                        Photo = product.Photo
                    };

                    db.Carts.Add(cartItem);
                    db.SaveChanges();

                    return Redirect("Show/" + product.Id);
                }
            }
            else
            {
                return NotFound();
            }

            
        }
    
    }
}
