using Microsoft.EntityFrameworkCore;
using ProductManagement.Application.Interfaces.Repositories;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .Include(x => x.RefreshTokens)
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<bool> ExistsAsync(string email)
    {
        return await _context.Users
            .AnyAsync(x => x.Email == email);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
    {
        return await _context.Users
            .Include(x => x.RefreshTokens)
            .FirstOrDefaultAsync(x =>
                x.RefreshTokens.Any(r => r.Token == refreshToken));
    }
}