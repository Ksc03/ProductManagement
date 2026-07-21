using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProductManagement.Application.Interfaces.Repositories;
using ProductManagement.Application.Interfaces.Services;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.Data;
using ProductManagement.Infrastructure.Data.Repositories;
using ProductManagement.Infrastructure.JWT;
using ProductManagement.Infrastructure.Services;


namespace ProductManagement.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));

        

        services.AddScoped<IProductRepository, ProductRepository>();

        services.AddScoped<IItemRepository, ItemRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        
        services.AddScoped<IUserRepository, UserRepository>();
        
        services.AddScoped<IPasswordService, PasswordService>();

        return services;
    }
}