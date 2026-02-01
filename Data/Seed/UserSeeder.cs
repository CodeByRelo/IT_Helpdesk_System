using Microsoft.AspNetCore.Identity;
using IT_Helpdesk_System.Models;

namespace IT_Helpdesk_System.Data.Seed
{
    public static class UserSeeder
    {
        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
        {
            const string adminEmail = "admin@helpdesk.local";
            const string adminPassword = "Admin@123";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FullName = "System Administrator"
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
