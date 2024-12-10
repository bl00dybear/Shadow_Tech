using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shadow_Tech.Data;
using Shadow_Tech.Models;

namespace Shadow_Tech.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ReviewsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Edit(int id)
        {
            var review = db.Reviews.Include(r => r.Product).FirstOrDefault(r => r.Id == id);

            if (review == null)
            {
                TempData["message"] = "Recenzia nu a fost găsită.";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }

            // Verifică dacă utilizatorul este autorul recenziei sau admin
            if (review.UserId != _userManager.GetUserId(User) && !User.IsInRole("Admin"))
            {
                TempData["message"] = "Nu aveți dreptul să editați această recenzie.";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }

            return View(review);
        }

        [HttpPost]
        public IActionResult Edit(Review review)
        {
            var existingReview = db.Reviews.Find(review.Id);

            if (ModelState.IsValid)
            {
                existingReview.Comment = review.Comment;
                existingReview.Rating = review.Rating;
                db.SaveChanges();

                TempData["message"] = "Recenzia a fost actualizată cu succes.";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Show", "Products", new { id = review.ProductId });
            }

            return RedirectToAction("Show", "Products", review.ProductId);
        }
        public IActionResult Delete(int id)
        {
            var review = db.Reviews.Find(id);

            if (review != null)
            {
                db.Reviews.Remove(review);
                db.SaveChanges();

                TempData["message"] = "Recenzia a fost ștearsă cu succes.";
                TempData["messageType"] = "alert-success";
            }
            else
            {
                TempData["message"] = "Recenzia nu a fost găsită.";
                TempData["messageType"] = "alert-danger";
            }

            return RedirectToAction("Show","Products",id); ;

        }
    }
}
