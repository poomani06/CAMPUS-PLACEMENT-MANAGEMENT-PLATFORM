
using CampusPlacementManagement.Constants;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace CampusPlacementManagement.Data;

public static class RoleSeeder
{
    public static async Task SeedRolesAsync(
        RoleManager<IdentityRole> roleManager)
    {
        string[] roles =
        {
            UserRoles.SuperAdmin,
            UserRoles.PlacementOfficer,
            UserRoles.CompanyHR,
            UserRoles.Student
        };

        foreach (var role in roles)
        {
            var roleExists = await roleManager.RoleExistsAsync(role);

            if (!roleExists)
            {
                var result = await roleManager.CreateAsync(
                    new IdentityRole(role));

                if (!result.Succeeded)
                {
                    throw new Exception(
                        $"Failed to create role: {role}");
                }
            }
        }
    }

    public static async Task SeedSuperAdminAsync(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
    {
        var email = configuration["AdminSeed:Email"];
        var password = configuration["AdminSeed:Password"];
        var fullName = configuration["AdminSeed:FullName"];

        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(fullName))
        {
            throw new InvalidOperationException(
                "SuperAdmin configuration is missing.");
        }

        var existingUser = await userManager.FindByEmailAsync(email);

        if (existingUser != null)
        {
            var isAdmin = await userManager.IsInRoleAsync(
                existingUser,
                UserRoles.SuperAdmin);

            if (!isAdmin)
            {
                var roleResult = await userManager.AddToRoleAsync(
                    existingUser,
                    UserRoles.SuperAdmin);

                if (!roleResult.Succeeded)
                {
                    throw new Exception(
                        "Failed to assign SuperAdmin role.");
                }
            }

            return;
        }

        var adminUser = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FullName = fullName,
            AccountStatus = AccountStatus.Approved,
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createResult = await userManager.CreateAsync(
            adminUser,
            password);

        if (!createResult.Succeeded)
        {
            var errors = string.Join(
                ", ",
                createResult.Errors.Select(e => e.Description));

            throw new Exception(
                $"Failed to create SuperAdmin: {errors}");
        }

        var assignRoleResult = await userManager.AddToRoleAsync(
            adminUser,
            UserRoles.SuperAdmin);

        if (!assignRoleResult.Succeeded)
        {
            var errors = string.Join(
                ", ",
                assignRoleResult.Errors.Select(e => e.Description));

            throw new Exception(
                $"Failed to assign SuperAdmin role: {errors}");
        }
    }
}
