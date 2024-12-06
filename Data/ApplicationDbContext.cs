
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shadow_Tech.Models;
using static Shadow_Tech.Models.OrderProduct;

namespace Shadow_Tech.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // definirea relatiei many-to-many dintre Article si Bookmark

            base.OnModelCreating(modelBuilder);

            // definire primary key compus
            modelBuilder.Entity<OrderProduct>()
                .HasKey(ab => new { ab.Id, ab.OrderId, ab.ProductId });


            // definire relatii cu modelele Bookmark si Article (FK)

            modelBuilder.Entity<OrderProduct>()
                .HasOne(ab => ab.Order)
                .WithMany(ab => ab.OrderProduct)
                .HasForeignKey(ab => ab.OrderId);

            modelBuilder.Entity<OrderProduct>()
                .HasOne(ab => ab.Product)
                .WithMany(ab => ab.OrderProduct)
                .HasForeignKey(ab => ab.ProductId);
        }
    }
}
