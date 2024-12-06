﻿using Microsoft.EntityFrameworkCore;

namespace Shadow_Tech.Models
{
	public class CartItem
	{
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		[Precision(18, 2)]
		public decimal Price { get; set; }
		public int Quantity {  get; set; }
		public decimal TotalPrice => Price*Quantity;
	}
}
