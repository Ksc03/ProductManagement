using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ProductManagement.API.Tests.Infrastructure;
using ProductManagement.Application.DTOs.Authentication;
using Xunit;

namespace ProductManagement.API.Tests.Controllers;

public class AuthControllerTests
{
    private readonly HttpClient _client;

    public AuthControllerTests()
    {
        var factory = new CustomWebApplicationFactory();
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_Should_Return_Success()
    {
        // Arrange
        var request = new RegisterRequestDto
        {
            UserName = "admin",
            Email = "admin@test.com",
            Password = "Password@123"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/v1/auth/register",
            request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Login_Should_Return_JwtToken_When_Credentials_Are_Valid()
    {
        // Arrange

        var register = new RegisterRequestDto
        {
            UserName = "john",
            Email = "john@test.com",
            Password = "Password@123"
        };

        await _client.PostAsJsonAsync(
            "/api/v1/auth/register",
            register);

        var login = new LoginRequestDto
        {
            Email = "john@test.com",
            Password = "Password@123"
        };

        // Act

        var response =
            await _client.PostAsJsonAsync(
                "/api/v1/auth/login",
                login);

        // Assert

        response.StatusCode.Should()
            .Be(HttpStatusCode.OK);

        var token =
            await response.Content.ReadFromJsonAsync<TokenResponseDto>();

        token.Should().NotBeNull();

        token!.AccessToken.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Register_Should_Return_BadRequest_When_Email_Already_Exists()
    {
        var request = new RegisterRequestDto
        {
            UserName = "admin",
            Email = "duplicate@test.com",
            Password = "Password@123"
        };

        await _client.PostAsJsonAsync(
            "/api/v1/auth/register",
            request);

        var response =
            await _client.PostAsJsonAsync(
                "/api/v1/auth/register",
                request);

        response.StatusCode.Should()
            .Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_Should_Return_BadRequest_When_Password_Is_Invalid()
    {
        var register = new RegisterRequestDto
        {
            UserName = "admin",
            Email = "admin123@test.com",
            Password = "Password@123"
        };

        await _client.PostAsJsonAsync(
            "/api/v1/auth/register",
            register);

        var login = new LoginRequestDto
        {
            Email = "admin123@test.com",
            Password = "WrongPassword"
        };

        var response =
            await _client.PostAsJsonAsync(
                "/api/v1/auth/login",
                login);

        response.StatusCode.Should()
            .Be(HttpStatusCode.BadRequest);
    }

}