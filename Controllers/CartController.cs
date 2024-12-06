using Microsoft.AspNetCore.Mvc;
using Shadow_Tech.Models;
using Shadow_Tech.Helpers;

namespace Shadow_Tech.Controllers
{
	public class CartController : Controller
	{
		private const string CartSessionKey = "Cart";
		public IActionResult Index()
		{
			//obtine produsele dein cos din sesiune
			var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();
			return View(cart);
		}

		public IActionResult AddToCart(int productId, string name, decimal price, int quantity = 1)
		{
			var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();

			var existingItem = cart.FirstOrDefault(i => i.ProductId == productId);
			if (existingItem != null)
			{
				existingItem.Quantity += quantity;
			}
			else
			{
				cart.Add(new CartItem
				{
					ProductId = productId,
					ProductName = name,
					Price = price,
					Quantity = quantity
				});
                _logger.LogInformation($"Adding to cart: {productId}, {name}, {price}");
            }

			HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);

			return RedirectToAction("Index", "Products");
		}

		public IActionResult RemoveFromCart(int productId)
		{
			var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();
			
			var itemToRemove = cart.FirstOrDefault(i => i.ProductId == productId);
			if (itemToRemove != null)
			{
				cart.Remove(itemToRemove);
			}

			HttpContext.Session.SetObjectAsJson(CartSessionKey,cart);

            return RedirectToAction("Index", "Products");
        }

		public IActionResult ClearCart()
		{
			HttpContext.Session.SetObjectAsJson(CartSessionKey, new List<CartItem>());
            return RedirectToAction("Index", "Products");
        }
	}
}
