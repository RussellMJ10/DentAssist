using DentAssistProyect.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace DentAssistProyect.Models.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context,
                                          UserManager<IdentityUser> userManager,
                                          RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            // Crear roles
            string[] roles = { "Administrador", "Recepcionista", "Odontologo" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Crear usuario admin
            var adminEmail = "admin@sonrisaplena.cl";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(adminUser, "Admin123!");
                await userManager.AddToRoleAsync(adminUser, "Administrador");
            }

            await context.SaveChangesAsync();
        }
    }
}
