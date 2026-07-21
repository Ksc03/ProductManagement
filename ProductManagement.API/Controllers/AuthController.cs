using Microsoft.AspNetCore.Mvc;
using ProductManagement.Application.Common;
using ProductManagement.Application.Common.Models;
using ProductManagement.Application.DTOs.Authentication;
using ProductManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;

namespace ProductManagement.API.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthController(
        IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequestDto dto)
    {
        await _authenticationService.RegisterAsync(dto);

        return Ok(
            ApiResponse<string>.SuccessResponse(
                "User registered successfully.",
                "Registration completed."));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequestDto dto)
    {
        var token = await _authenticationService.LoginAsync(dto);

        return Ok(
            ApiResponse<TokenResponseDto>.SuccessResponse(
                token,
                "Login successful."));
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(
    RefreshTokenRequestDto dto)
    {
        var token = await _authenticationService
            .RefreshTokenAsync(dto);

        return Ok(
            ApiResponse<TokenResponseDto>
                .SuccessResponse(
                    token,
                    "Token refreshed successfully."));
    }
}