using IT_Helpdesk_System.Models;
using Microsoft.AspNetCore.Identity;

namespace IT_Helpdesk_System.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // 1️⃣ Ensure roles exist
            string[] roles = { "Admin", "Technician", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2️⃣ Optionally create a default admin
            string adminEmail = "admin@helpdesk.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Administrator"
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!"); // strong password
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
