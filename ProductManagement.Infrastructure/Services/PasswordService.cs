using Microsoft.AspNetCore.Identity;
using ProductManagement.Application.Interfaces.Services;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Infrastructure.Services;

public class PasswordService : IPasswordService
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public string HashPassword(string password)
    {
        var user = new User();

        return _passwordHasher.HashPassword(user, password);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        var user = new User();

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            passwordHash,
            password);

        return result == PasswordVerificationResult.Success;
    }
}