using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shadow_Tech.Data;
using Shadow_Tech.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            var cartItems = db.Carts.Where(i => i.UserId == 1).ToList();
            return View(cartItems);
        }


        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            Cart cart = db.Carts.Where(ci => ci.Id == id).First();

            db.Carts.Remove(cart);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // Golirea coșului
        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            //string userId = User.Identity.Name;
            var userId = 1; // Pentru a simplifica, vom folosi un UserId fictiv
            var userCart = db.Carts.Where(ci => ci.UserId == userId);
            db.Carts.RemoveRange(userCart);
            await db.SaveChangesAsync();

            return Json(new { success = true, message = "Coșul a fost golit!" });
        }
    }
}
