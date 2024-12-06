using Microsoft.EntityFrameworkCore;

namespace Shadow_Tech.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public int Stock { get; set; }          //salvam stocul valabil al acestui produs
        public string? Photo { get; set; }          //salvam URL-ul pozei
        public bool Listed { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public virtual ICollection<OrderProduct>? OrderProduct { get; set; }
    }
}
