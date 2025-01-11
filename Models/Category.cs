using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shadow_Tech.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(50, ErrorMessage = "Title should have less than 50 characters")]
        [MinLength(5, ErrorMessage = "Title should hame more than 5 characters")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Category description is required")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Category image is required")]
        public string? Image { get; set; }
        public ICollection<Product>? Products { get; set; }
        
    }
}
