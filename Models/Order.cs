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

        public virtual ApplicationUser? User { get; set; }

        public virtual ICollection<OrderProduct>? OrderProduct { get; set; }
    }
}
