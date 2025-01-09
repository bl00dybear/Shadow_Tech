using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Shadow_Tech.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
        [Precision (18,2)]
        public decimal Total {  get; set; }
        public string? UserId { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public bool CardPayment { get; set; }
        public virtual ApplicationUser? User { get; set; }

        public virtual ICollection<OrderProduct>? OrderProduct { get; set; }
    }
}
