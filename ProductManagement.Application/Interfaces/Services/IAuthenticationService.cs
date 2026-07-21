using ProductManagement.Application.DTOs.Authentication;

namespace ProductManagement.Application.Interfaces.Services;

public interface IAuthenticationService
{
    Task RegisterAsync(RegisterRequestDto dto);

    Task<TokenResponseDto> LoginAsync(LoginRequestDto dto);

    Task<TokenResponseDto> RefreshTokenAsync(
    RefreshTokenRequestDto dto);
}