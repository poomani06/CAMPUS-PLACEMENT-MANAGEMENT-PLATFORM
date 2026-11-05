using CampusPlacementManagement.Constants;
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
            var roleExists = await roleManager.RoleExistsAsync(
                role);

            if (!roleExists)
            {
                await roleManager.CreateAsync(
                    new IdentityRole(role));
            }
        }
    }
}