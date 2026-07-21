
using ProductManagement.Application.DTOs.Authentication;
using ProductManagement.Application.Interfaces.Repositories;
using ProductManagement.Application.Interfaces.Services;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Exceptions;

namespace ProductManagement.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordService _passwordService;

    public AuthenticationService(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IJwtTokenService jwtTokenService,
    IPasswordService passwordService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
        _passwordService = passwordService;
    }

    public async Task RegisterAsync(RegisterRequestDto dto)
    {
        if (await _userRepository.ExistsAsync(dto.Email))
        {
            throw new BadRequestException(
                "User already exists.");
        }

        var user = new User
        {
            UserName = dto.UserName,
            Email = dto.Email,
            Role = "User"
        };

        user.PasswordHash =
            _passwordService.HashPassword(dto.Password);

        await _userRepository.AddAsync(user);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<TokenResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user == null)
        {
            throw new BadRequestException(
                "Invalid email or password.");
        }

        var isValid = _passwordService.VerifyPassword(
            dto.Password,
            user.PasswordHash);

        if (!isValid)
        {
            throw new BadRequestException(
                "Invalid email or password.");
        }

        var token = _jwtTokenService.GenerateTokens(user);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = token.RefreshToken,
            ExpiresOn = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
        });

        await _unitOfWork.SaveChangesAsync();

        return token;
    }

    public async Task<TokenResponseDto> RefreshTokenAsync(
    RefreshTokenRequestDto dto)
    {
        var user = await _userRepository.GetByRefreshTokenAsync(dto.RefreshToken);

        if (user == null)
        {
            throw new BadRequestException("Invalid refresh token.");
        }

        var storedToken = user.RefreshTokens
            .First(x => x.Token == dto.RefreshToken);

        if (storedToken.IsRevoked ||
            storedToken.ExpiresOn <= DateTime.UtcNow)
        {
            throw new BadRequestException("Refresh token has expired.");
        }

        storedToken.IsRevoked = true;

        var token = _jwtTokenService.GenerateTokens(user);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = token.RefreshToken,
            ExpiresOn = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
        });

        await _unitOfWork.SaveChangesAsync();

        return token;
    }
}