using Microsoft.AspNetCore.Identity;
using Security.Identity;

namespace Shopping.Security
{
    public static class AppRoles
    {
        public const string Admin = "admin";
        public const string User = "User";
    }

    public static class IdentityDataSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            foreach (var roleName in new[] { AppRoles.Admin, AppRoles.User })
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = roleName,
                        Description = roleName
                    });
                }
            }
        }
    }
}
