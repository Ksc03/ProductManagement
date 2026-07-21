using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ProductManagement.API.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");

        var key = Encoding.UTF8.GetBytes(
            jwtSettings["SecretKey"]!);

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtSettings["Issuer"],

                        ValidAudience = jwtSettings["Audience"],

                        IssuerSigningKey =
                            new SymmetricSecurityKey(key),

                        ClockSkew = TimeSpan.Zero
                    };
            });

        services.AddAuthorization();

        return services;
    }
}