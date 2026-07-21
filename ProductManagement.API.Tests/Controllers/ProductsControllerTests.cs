using FluentAssertions;
using ProductManagement.API.Tests.Infrastructure;
using System.Net;
using Xunit;

namespace ProductManagement.API.Tests.Controllers;

public class ProductsControllerTests
{
    private readonly HttpClient _client;

    public ProductsControllerTests()
    {
        var factory = new CustomWebApplicationFactory();

        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetProducts_Should_Return_Unauthorized_When_User_Is_Not_Authenticated()
    {
        // Act

        var response =
            await _client.GetAsync("/api/v1/products");

        // Assert

        response.StatusCode.Should()
            .Be(HttpStatusCode.Unauthorized);
    }


}