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
            ViewBag.Date = DateTime.Today.AddDays(2).Date;
            return View(cartItems);
        }

        public IActionResult CheckOut()
        {
            var cartItems = db.Carts.Where(i => i.UserId == _userManager.GetUserId(User)).ToList();
            for (int i = 0; i < cartItems.Count(); i++)
            {
                if(cartItems[i].ProductName.Length >=15)
                    cartItems[i].ProductName = cartItems[i].ProductName.Substring(0, 15)+"...";
            }
            
            ViewBag.CartItems = cartItems;
            var order= new Order();
            return View(order);
        }


        [HttpPost]
        public IActionResult CheckOut(Order neworder)
        {
            var cartItems = db.Carts.Where(i => i.UserId == _userManager.GetUserId(User)).ToList();
            // Step 1: Get the list of product IDs from the cart items
            var productIds = cartItems.Select(ci => ci.ProductId).ToList();

            // Step 2: Query the database with these IDs
            var productList = db.Products
                .Where(p => productIds.Contains(p.Id))
                .ToList();

            foreach (var product in productList)
            {
                if (product.Stock < cartItems.Where(ci => ci.ProductId == product.Id).Sum(ci => ci.Quantity))
                {
                    ModelState.AddModelError("Stock", "There is no more stock for" + product.Title + ". Only "+ product.Stock+" products available.");
                    return View(neworder);
                }
            }
            foreach (var product in productList)
            {
                product.Stock -= cartItems.Where(ci => ci.ProductId == product.Id).Sum(ci => ci.Quantity);
            }
            Order order = new Order
            {
                UserId = _userManager.GetUserId(User),
                Address = neworder.Address,
                City = neworder.City,
                PostalCode = neworder.PostalCode,
                Date = DateTime.Now,
                Email = neworder.Email,
                Name = neworder.Name,
                Phone = neworder.Phone,
                Country = neworder.Country,
                Total = cartItems.Sum(ci => ci.Price * ci.Quantity),
                CardPayment = neworder.CardPayment,
                OrderProduct = cartItems.Select(ci => new OrderProduct
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity

                }).ToList()
            };
            db.Orders.Add(order);
            db.Carts.RemoveRange(cartItems);
            db.SaveChanges();
            return RedirectToAction("Index");
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

            return Json(new { success = true, message = "Cart is empty!" });
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
