using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Application.Interfaces.Services;
using ProductManagement.Application.Mapping;
using ProductManagement.Application.Services;
using ProductManagement.Application.Validators.Product;

namespace ProductManagement.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ProductProfile));

        services.AddScoped<IProductService, ProductService>();

        services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();

        services.AddScoped<IAuthenticationService, AuthenticationService>();

        return services;
    }
}