using LSMS.Infrastructure.Identity;
using LSMS.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LSMS.API.AppService
{
    public static class IdentityService
    {
        public static IServiceCollection ApplyIdentityServices(this IServiceCollection Services, IConfiguration configuration)
        {
            var jwtKey = configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is missing.");
            var issuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is missing.");
            var audience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is missing.");

            Services.AddDataProtection();
            Services.AddIdentityCore<AppUser>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan =
                    TimeSpan.FromMinutes(15);
                options.Lockout.AllowedForNewUsers = true;
            })
              .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<LSMSDbContext>()
                .AddDefaultTokenProviders()
            .AddSignInManager()
            .AddDefaultTokenProviders();

            Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                JwtBearerDefaults.AuthenticationScheme;

                options.DefaultChallengeScheme =
                JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(30)
            };
            });

            Services.AddAuthorization();
            return Services;
        }

    }
}
