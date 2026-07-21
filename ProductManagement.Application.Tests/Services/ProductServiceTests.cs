using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using ProductManagement.Application.DTOs.Product;
using ProductManagement.Application.Interfaces.Repositories;
using ProductManagement.Application.Mapping;
using ProductManagement.Application.Services;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Exceptions;
using Xunit;

public class ProductServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;

    private readonly Mock<IValidator<CreateProductDto>> _createValidatorMock;

    private readonly Mock<IValidator<UpdateProductDto>> _updateValidatorMock;

    private readonly Mock<ILogger<ProductService>> _loggerMock;

    private readonly IMapper _mapper;

    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _productRepositoryMock = new Mock<IProductRepository>();

        _createValidatorMock =
            new Mock<IValidator<CreateProductDto>>();

        _updateValidatorMock =
            new Mock<IValidator<UpdateProductDto>>();

        _loggerMock =
            new Mock<ILogger<ProductService>>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductProfile>();
        });

        _mapper = config.CreateMapper();

        _unitOfWorkMock
            .Setup(x => x.Products)
            .Returns(_productRepositoryMock.Object);

        _productService = new ProductService(
            _unitOfWorkMock.Object,
            _mapper,
            _createValidatorMock.Object,
            _updateValidatorMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task CreateAsync_Should_Create_Product_When_Request_Is_Valid()
    {
        // Arrange

        var dto = new CreateProductDto
        {
            ProductName = "Laptop"
        };

        _createValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act

        await _productService.CreateAsync(dto);

        // Assert

        _productRepositoryMock.Verify(
            x => x.AddAsync(It.IsAny<Product>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Product_When_Product_Exists()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            ProductName = "Laptop",
            CreatedBy = "System",
            CreatedOn = DateTime.UtcNow
        };

        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(product);

        // Act
        var result = await _productService.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Laptop", result.ProductName);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Throw_NotFoundException_When_Product_Does_Not_Exist()
    {
        // Arrange

        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((Product?)null);

        // Act

        var action = async () =>
            await _productService.GetByIdAsync(1);

        // Assert

        await Assert.ThrowsAsync<NotFoundException>(action);
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_Product()
    {
        // Arrange

        var product = new Product
        {
            Id = 1,
            ProductName = "Laptop"
        };

        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(product);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act

        await _productService.DeleteAsync(1);

        // Assert

        _productRepositoryMock.Verify(
            x => x.Delete(product),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Product()
    {
        // Arrange

        var product = new Product
        {
            Id = 1,
            ProductName = "Old Product"
        };

        var dto = new UpdateProductDto
        {
            ProductName = "New Product"
        };

        _updateValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        _productRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(product);

        _unitOfWorkMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act

        await _productService.UpdateAsync(1, dto);

        // Assert

        _productRepositoryMock.Verify(
            x => x.Update(It.IsAny<Product>()),
            Times.Once);

        _unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);

        Assert.Equal("New Product", product.ProductName);
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_ValidationException_When_Request_Is_Invalid()
    {
        // Arrange

        var dto = new CreateProductDto();

        var validationResult = new ValidationResult(
        [
            new ValidationFailure(
            "ProductName",
            "Product Name is required")
        ]);

        _createValidatorMock
            .Setup(x => x.ValidateAsync(dto, default))
            .ReturnsAsync(validationResult);

        // Act

        var action = async () =>
            await _productService.CreateAsync(dto);

        // Assert

        await Assert.ThrowsAsync<AppValidationException>(action);
    }
}