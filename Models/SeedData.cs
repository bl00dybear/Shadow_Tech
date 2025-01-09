using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shadow_Tech.Data;

namespace Shadow_Tech.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                // Verificăm dacă există roluri, dacă da, ieșim
                if (context.Roles.Any())
                {
                    return;
                }

                // CREAREA ROLURILOR
                string[] roleNames = { "Admin", "Contribuitor", "User" };
                foreach (var roleName in roleNames)
                {
                    if (!roleManager.RoleExistsAsync(roleName).Result)
                    {
                        roleManager.CreateAsync(new IdentityRole(roleName)).Wait();
                    }
                }

                // CREAREA UTILIZATORILOR
                var adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@test.com",
                    EmailConfirmed = true
                };
                var contribuitorUser = new ApplicationUser
                {
                    UserName = "contribuitor@test.com",
                    Email = "contribuitor@test.com",
                    EmailConfirmed = true
                };
                var normalUser = new ApplicationUser
                {
                    UserName = "user@test.com",
                    Email = "user@test.com",
                    EmailConfirmed = true
                };

                if (userManager.FindByEmailAsync(adminUser.Email).Result == null)
                {
                    userManager.CreateAsync(adminUser, "Admin1!").Wait();
                    userManager.AddToRoleAsync(adminUser, "Admin").Wait();
                }

                if (userManager.FindByEmailAsync(contribuitorUser.Email).Result == null)
                {
                    userManager.CreateAsync(contribuitorUser, "Contribuitor1!").Wait();
                    userManager.AddToRoleAsync(contribuitorUser, "Contribuitor").Wait();
                }

                if (userManager.FindByEmailAsync(normalUser.Email).Result == null)
                {
                    userManager.CreateAsync(normalUser, "User1!").Wait();
                    userManager.AddToRoleAsync(normalUser, "User").Wait();
                }

                // Salvăm modificările
                context.SaveChanges();
            }
        }
    }
}