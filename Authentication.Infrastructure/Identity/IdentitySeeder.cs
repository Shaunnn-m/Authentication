using Authentication.Application.Common.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Authentication.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
        RoleManager<IdentityRole<Guid>> roleManager,
        UserManager<ApplicationUser> userManager,
        IOptions<InitialAdminOptions> initialAdminOptions)
    {
        await SeedRolesAsync(roleManager);

        await SeedInitialAdminAsync(
            userManager,
            initialAdminOptions.Value);
    }

    private static async Task SeedRolesAsync(
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        var roles = new[]
        {
            AppRoles.Admin,
            AppRoles.Customer
        };

        foreach (var role in roles)
        {
            if (await roleManager.RoleExistsAsync(role))
            {
                continue;
            }

            var result = await roleManager.CreateAsync(
                new IdentityRole<Guid>(role));

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(error => error.Description));

                throw new InvalidOperationException(
                    $"Failed to create role '{role}': {errors}");
            }
        }
    }

    private static async Task SeedInitialAdminAsync(
        UserManager<ApplicationUser> userManager,
        InitialAdminOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Email))
        {
            throw new InvalidOperationException(
                "Initial admin email is not configured.");
        }

        if (string.IsNullOrWhiteSpace(options.Password))
        {
            throw new InvalidOperationException(
                "Initial admin password is not configured.");
        }

        var existingUser =
            await userManager.FindByEmailAsync(options.Email);

        if (existingUser is not null)
        {
            if (!await userManager.IsInRoleAsync(
                    existingUser,
                    AppRoles.Admin))
            {
                var roleResult =
                    await userManager.AddToRoleAsync(
                        existingUser,
                        AppRoles.Admin);

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        roleResult.Errors.Select(
                            error => error.Description));

                    throw new InvalidOperationException(
                        $"Failed to assign Admin role: {errors}");
                }
            }

            return;
        }

        var admin = new ApplicationUser
        {
            UserName = options.Email,
            Email = options.Email,
            EmailConfirmed = true,
            IsActive = true,
            FirstName = options.FirstName,
            LastName = options.LastName
        };

        var createResult =
            await userManager.CreateAsync(
                admin,
                options.Password);

        if (!createResult.Succeeded)
        {
            var errors = string.Join(
                ", ",
                createResult.Errors.Select(
                    error => error.Description));

            throw new InvalidOperationException(
                $"Failed to create initial admin: {errors}");
        }

        var roleAssignmentResult =
            await userManager.AddToRoleAsync(
                admin,
                AppRoles.Admin);

        if (!roleAssignmentResult.Succeeded)
        {
            var errors = string.Join(
                ", ",
                roleAssignmentResult.Errors.Select(
                    error => error.Description));

            throw new InvalidOperationException(
                $"Failed to assign Admin role: {errors}");
        }
    }
}