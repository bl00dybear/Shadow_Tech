using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Shadow_Tech.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName {  get; set; }
        public string? LastName { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
        public IEnumerable<SelectListItem>? AllRoles { get; set; }
    }
}
