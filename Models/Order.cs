using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shadow_Tech.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
        [Precision (18,2)]
        public decimal Total {  get; set; }
        public string? UserId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Adress is required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        [Required(ErrorMessage = "PostralCode is required")]
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Paying method is required")]
        public bool CardPayment { get; set; }
        public virtual ApplicationUser? User { get; set; }

        public virtual ICollection<OrderProduct>? OrderProduct { get; set; }
    }
}
