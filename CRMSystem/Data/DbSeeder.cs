using Microsoft.AspNetCore.Identity;

namespace CRMSystem.Data
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            string[] roles =
            {
                "SuperAdmin",
                "Admin",
                "SalesManager",
                "SalesRep",
                "Support",
                "Viewer"
            };

            foreach (var role in roles)
            {
                var roleExists =
                    await roleManager.RoleExistsAsync(role);

                if (!roleExists)
                {
                    await roleManager.CreateAsync(
                        new IdentityRole<Guid>(role));
                }
            }
        }
    }
    public static class UserSeeder
    {
        public static async Task SeedAsync(
            UserManager<ApplicationUser> userManager)
        {
            const string email = "superadmin";
            const string password = "Admin@123456";

            var existingUser =
                await userManager.FindByEmailAsync(email);

            if (existingUser != null)
                return;

            var superAdmin = new ApplicationUser
            {
                OrganizationId = null,
                FirstName = "Super",
                LastName = "Admin",
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                Status = "ACTIVE",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var result =
                await userManager.CreateAsync(
                    superAdmin,
                    password);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(x => x.Description));

                throw new Exception(
                    $"Failed to create SuperAdmin: {errors}");
            }

            var roleResult =
                await userManager.AddToRoleAsync(
                    superAdmin,
                    "SuperAdmin");

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    roleResult.Errors.Select(x => x.Description));

                throw new Exception(
                    $"Failed to assign SuperAdmin role: {errors}");
            }
        }
    }

}
