using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shadow_Tech.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(512, ErrorMessage = "Titlul nu poate avea mai mult de 512 caractere")]
        [MinLength(3, ErrorMessage = "Titlul trebuie sa aiba mai mult de 3 caractere")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Product description is required")]
        public string? Description { get; set; }
        [Precision(18, 2)]
        [Range(0.01, 100000000, ErrorMessage = "Price should be between 0.01 and 100000000")]
        public decimal Price { get; set; }
        [Precision(18, 2)]
        public decimal ProductRating { get; set; }
        [Range(1, 100000000, ErrorMessage = "Stock should be between 1 and 100000000")]
        public int Stock { get; set; }
        [Required(ErrorMessage = "Product photo is required")]
        public string? Photo { get; set; }
        public bool Listed { get; set; }
        [Required(ErrorMessage = "Select category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public ICollection<Review>? Reviews { get; set; }
        public virtual ICollection<OrderProduct>? OrderProduct { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem>? Categ { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem>? Rev {  get; set; }

    }
}
