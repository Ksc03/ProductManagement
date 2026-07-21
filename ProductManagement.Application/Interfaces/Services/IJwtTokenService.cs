using ProductManagement.Application.DTOs.Authentication;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Application.Interfaces.Services;

public interface IJwtTokenService
{
    TokenResponseDto GenerateTokens(User user);

    string GenerateRefreshToken();
}