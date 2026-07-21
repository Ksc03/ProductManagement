using ProductManagement.Domain.Entities;

namespace ProductManagement.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);

    Task<bool> ExistsAsync(string email);

    Task AddAsync(User user);

    Task<User?> GetByRefreshTokenAsync(string refreshToken);
}