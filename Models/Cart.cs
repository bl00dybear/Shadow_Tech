﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Shadow_Tech.Models
{
	public class Cart
	{
        public int Id { get; set; }
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Numele produsului este obligatoriu")]
        public string? ProductName { get; set; }
		[Precision(18, 2)]
		public decimal Price { get; set; }
		public int Quantity {  get; set; }
		public string UserId { get; set; }
        
        public string? Photo { get; set; }
        public decimal TotalPrice => Price*Quantity;
	}
}
