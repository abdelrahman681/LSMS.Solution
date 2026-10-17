using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Infrastructure.Identity
{
    public static class AdminSeeder
    {
       
        public static async Task SeedAdminAsync(UserManager<AppUser> userManager,IConfiguration configuration)
        {
            var email = configuration["AdminSeed:Email"];
            var password = configuration["AdminSeed:Password"];
            var fullName = configuration["AdminSeed:FullName"];

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(fullName))
            {
                throw new InvalidOperationException(
                    "AdminSeed settings are missing.");
            }

            var user = await userManager.FindByEmailAsync(email);

            if (user is null)
            {
                user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    FullName = fullName,
                    EmailConfirmed = true,
                    IsActive = true
                };

                var createResult = await userManager.CreateAsync(
                    user, password);

                if (!createResult.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        createResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(
                        $"Admin creation failed: {errors}");
                }
            }

            if (!await userManager.IsInRoleAsync(user, "Admin"))
            {
                var roleResult = await userManager.AddToRoleAsync(
                    user, "Admin");

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(
                        "; ",
                        roleResult.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(
                        $"Admin role assignment failed: {errors}");
                }
            }
        }
    }

}

