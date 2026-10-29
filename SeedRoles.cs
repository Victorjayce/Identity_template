using Identity_template.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity_template
{
    public static class SeedRoles
    {
        public static async Task SeedRole(WebApplication? app)
    {
        if (app is null) return;
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<User>>(); // your user class
            var config = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<Program>>();

            foreach (var role in new[] { UserRoles.Admin, UserRoles.User })
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // 2. Seed default admin, remove if you don't want a default admin
            var adminEmail = config["SeedAdmin:Email"];
            var adminPassword = config["SeedAdmin:Password"];
            var adminUserName = config["SeedAdmin:UserName"] ?? "Admin";

            if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
            {
                logger.LogWarning("SeedAdmin config missing — skipping admin user seed.");
            }
            else
            {
                var admin = await userManager.FindByEmailAsync(adminEmail);

                if (admin is null)
                {
                    admin = new User
                    {
                        UserName = adminUserName,
                        Email = adminEmail,
                        EmailConfirmed = true,
                        // set any required custom fields here (GlobalRole, DisplayName, etc.)
                    };

                    var createResult = await userManager.CreateAsync(admin, adminPassword);

                    if (!createResult.Succeeded)
                    {
                        logger.LogError("Failed to create seed admin: {Errors}",
                            string.Join("; ", createResult.Errors.Select(e => e.Description)));
                    }
                    else
                    {
                        logger.LogInformation("Seed admin created: {Email}", adminEmail);
                    }
                }

                if (admin is not null && !await userManager.IsInRoleAsync(admin, UserRoles.Admin))
                {
                    var roleResult = await userManager.AddToRoleAsync(admin, UserRoles.Admin);
                    if (!roleResult.Succeeded)
                    {
                        await userManager.DeleteAsync(admin);
                        logger.LogError("Failed to assign Admin role: {Errors}",
                            string.Join("; ", roleResult.Errors.Select(e => e.Description)));
                    }
                }
            }
        }
    }

    }
}
