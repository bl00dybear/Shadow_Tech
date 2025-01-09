using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            SetAccessRights();

            // Interogare pentru categorii și numărul de produse asociate
            var categoriesWithProductCount = db.Categories
                .Select(category => new
                {
                    category.Id,
                    category.Name,
                    category.Image,
                    category.Description,
                    ProductCount = category.Products.Count(product => product.Listed) // Numărul de produse asociate
                })
                .OrderBy(c => c.Name)
                .ToList();

            // Transmiterea datelor către view prin ViewBag
            ViewBag.Categories = categoriesWithProductCount;

            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult New()
        {
            
            Category category = new Category();
            return View(category);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult New(Category category)
        {
            if (ModelState.IsValid)
            {
                

                db.Categories.Add(category);
                db.SaveChanges();
                TempData["message"] = "Categoria a fost adaugata";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");
            }
            else
            {
                
                return View(category);
            }
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            Category category = db.Categories.Where(art=>art.Id  == id).FirstOrDefault();
            return View(category);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, Category requestCategory)
        {
            Category category = db.Categories.Find(id);

            if (ModelState.IsValid)
            {
               
                    category.Name = requestCategory.Name;
                    category.Description = requestCategory.Description;
                    category.Image = requestCategory.Image;
                    TempData["message"] = "Categoria a fost modificata";
                    TempData["messageType"] = "alert-success";
                    db.SaveChanges();
                    return RedirectToAction("Index");
                
                
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles ="Admin")]
        public ActionResult Delete(int id)
        {
            // Find the category including its related products
            Category category = db.Categories.Include("Products")
                                             .Where(cat => cat.Id == id)
                                             .FirstOrDefault();

            if (category == null)
            {
                TempData["message"] = "Categoria nu a fost găsită.";
                TempData["messageType"] = "alert-danger";
                Console.WriteLine("Succes");
                return RedirectToAction("Index");
            }

            if (User.IsInRole("Admin"))
            {
                // Update the products to unlisted
                foreach (var product in category.Products)
                {
                    product.Listed = false;
                }

                // Remove the category
                db.Categories.Remove(category);

                // Save changes
                db.SaveChanges();
                

                TempData["message"] = "Categoria a fost ștearsă, iar produsele asociate au fost marcate ca nelistate.";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveți dreptul să ștergeți această categorie.";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }
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
    }
}
