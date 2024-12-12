using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public CartsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Authorize(Roles = "Contribuitor,Admin,User")]
        public IActionResult Index()
        {
            var cartItems = db.Carts.Where(i => i.UserId == _userManager.GetUserId(User)).ToList();
            return View(cartItems);
        }

        public IActionResult CheckOut()
        {
            var cartItems = db.Carts.Where(i => i.UserId == _userManager.GetUserId(User)).ToList();
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
            var userId = _userManager.GetUserId(User); // Pentru a simplifica, vom folosi un UserId fictiv
            var userCart = db.Carts.Where(ci => ci.UserId == userId);
            db.Carts.RemoveRange(userCart);
            await db.SaveChangesAsync();

            return Json(new { success = true, message = "Coșul a fost golit!" });
        }

        [HttpPost]
        public IActionResult IncrementQuantity(int id)
        {
            Cart cart = db.Carts.Where(ci => ci.Id == id).First();
            cart.Quantity++;
            db.SaveChanges();
            ViewBag.CartTotal = TotalCart(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DecrementQuantity(int id)
        {
            Cart cart = db.Carts.Where(ci => ci.Id == id).First();
            cart.Quantity--;
            db.SaveChanges();
            ViewBag.CartTotal=TotalCart(id);



            return RedirectToAction("Index");
        }

        [HttpPost]
        public decimal TotalCart(int id)
        {
            Cart cart = db.Carts.Where(ci => ci.Id == id).First();

            //Console.WriteLine("aici");

            string userId = cart.UserId;
            decimal totalSum = db.Carts
                .Where(ci => ci.UserId == userId)
                .Sum(ci => ci.Price*ci.Quantity);
            //ViewBag.CartTotal = totalSum;

            return totalSum;
        }
    }
}
