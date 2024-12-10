using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shadow_Tech.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [Required(ErrorMessage = "Category description is required")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Category image is required")]
        public string? Image { get; set; }
        public ICollection<Product>? Products { get; set; }
        
    }
}
