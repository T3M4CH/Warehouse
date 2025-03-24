using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using Warehouse.Data;
using Warehouse.Services;
using Warehouse.Services.Interfaces;
using Warehouse.Token;
using Warehouse.Token.Interfaces;
using Warehouse.UnitOfWork.Interfaces;
using WarehouseApi.Entities;
using WarehouseApi.Repositories;
using WarehouseApi.Repositories.Interfaces;

namespace Warehouse.Helpers;

public static class ApplicationServiceExtension
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();
        builder.Services.AddDbContext<DataContext>
            (options => options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection")));

        builder.Services.AddIdentity<UserEntity, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 5;
            })
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
            });
        
        builder.Services.AddAuthorization();

        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        builder.Services.AddScoped<IContainerRepository, ContainerRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        builder.Services.AddScoped<IContainerService, ContainerService>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IWarehouseService, WarehouseService>();
        
        builder.Logging.ClearProviders(); 
        builder.Host.UseNLog(); 
        
        return builder;
    }
}