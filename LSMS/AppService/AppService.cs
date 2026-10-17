using LSMS.API.Errors;
using LSMS.Application.Interfaces.Auth;
using LSMS.Application.Interfaces.Category;
using LSMS.Application.Interfaces.Image;
using LSMS.Application.Interfaces.Product;
using LSMS.Infrastructure.Identity;
using LSMS.Infrastructure.Persistence;
using LSMS.Infrastructure.Services.AuthServices;
using LSMS.Infrastructure.Services.CategoryService;
using LSMS.Infrastructure.Services.Images;
using LSMS.Infrastructure.Services.ProductService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using System.Text.Json.Serialization;

namespace LSMS.API.AppService
{
    public static class AppService
    {
        public static IServiceCollection ApplyServices(this IServiceCollection Services, IConfiguration configuration)
        {
            Services.AddScoped<IAuthService, AuthService>();
            Services.AddScoped<ICategoryService, CategoryService>();
            Services.AddScoped<IProductService, ProductService>();
            Services.AddScoped<IProductImageStorage, ProductImageStorage>();
            Services.AddDbContext<LSMSDbContext>(options => options.UseSqlServer
            (configuration.GetConnectionString("DefaultConnection")));
            Services.Configure<ApiBehaviorOptions>(option =>
            {
                option.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Any())
                                              .SelectMany(p => p.Value.Errors)
                                              .Select(e => e.ErrorMessage).ToList();

                    var ApiValidationRespons = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ApiValidationRespons);
                };
            });
            //Services.AddAutoMapper(cfg =>
            //{
            //}, typeof(MapingProfiles).Assembly);
            Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters
                .Add(new JsonStringEnumConverter());
            });
            Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Store99 E-Commerce API",
                    Version = "v1"
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter Bearer Token"
                });
                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });
            });
            return Services;
        }

    }
}
