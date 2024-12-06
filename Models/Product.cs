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

        [Required(ErrorMessage = "Titlul este obligatoriu")]
        [StringLength(512, ErrorMessage = "Titlul nu poate avea mai mult de 512 caractere")]
        [MinLength(3, ErrorMessage = "Titlul trebuie sa aiba mai mult de 3 caractere")]
        public string? Title { get; set; }
        [Required(ErrorMessage = "Descrierea produsului este obligatorie")]
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
        [NotMapped]
        public IEnumerable<SelectListItem>? Categ { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem>? Rev {  get; set; }

    }
}
