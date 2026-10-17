using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Infrastructure.Identity
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager) 
        {
            string[] roles = ["Admin", "Cashier"];

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync( new IdentityRole(role));
                    if (!result.Succeeded)
                    {
                        var errors = string.Join(
                            ", ",
                            result.Errors.Select(e => e.Description));

                        throw new InvalidOperationException(
                            $"Failed to create role {role}: {errors}");
                    }
                }
            }

        }
    }
}
