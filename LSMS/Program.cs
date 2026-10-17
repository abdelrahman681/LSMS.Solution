using LSMS.API.AppService;
using LSMS.Infrastructure.Identity;
using LSMS.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LSMS
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.ApplyServices(builder.Configuration);
            builder.Services.ApplyIdentityServices(builder.Configuration);
            var app = builder.Build();
            #region Update DateBase Outo
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var LoggeFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await IdentitySeeder.SeedRolesAsync(roleManager);
                var userManager =services.GetRequiredService<UserManager<AppUser>>();
                await AdminSeeder.SeedAdminAsync(userManager,app.Configuration);
                var context = services.GetRequiredService<LSMSDbContext>();
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                var logger = LoggeFactory.CreateLogger<Program>();
                logger.LogError(ex, ex.Message);
                //logger.LogError(ex, "An error has been  occured during apply the migration ");
            }
            #endregion
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()||app.Environment.IsProduction())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
