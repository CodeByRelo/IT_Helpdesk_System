using Microsoft.AspNetCore.Identity;

namespace IT_Helpdesk_System.Data.Seed
{
    public static class RoleSeeder
    {
        private static readonly string[] Roles =
        {
            "Admin",
            "ITSupport",
            "User"
        };

        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
