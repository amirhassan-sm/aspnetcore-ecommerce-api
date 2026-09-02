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
        private const string DefaultAdminUserName = "amirhassan";
        private const string DefaultAdminPassword = "83488348H@ssan";

        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

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

            var admin = await userManager.FindByNameAsync(DefaultAdminUserName);
            if (admin is null)
            {
                admin = new ApplicationUser
                {
                    UserName = DefaultAdminUserName,
                    FirstName = "Amirhassan",
                    LastName = "Admin",
                };

                var created = await userManager.CreateAsync(admin, DefaultAdminPassword);
                if (!created.Succeeded)
                {
                    var errors = string.Join("; ", created.Errors.Select(e => e.Description));
                    Console.WriteLine($"[Seed] Failed to create admin user '{DefaultAdminUserName}': {errors}");
                    return;
                }
            }

            if (!await userManager.IsInRoleAsync(admin, AppRoles.Admin))
            {
                await userManager.AddToRoleAsync(admin, AppRoles.Admin);
            }
        }
    }
}
