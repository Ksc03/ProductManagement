using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Application.Interfaces.Repositories;
using ProductManagement.Infrastructure.Data;
using ProductManagement.Infrastructure.Data.Repositories;


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

        return services;
    }
}