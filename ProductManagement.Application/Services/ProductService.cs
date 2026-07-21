using AutoMapper;
using FluentValidation;
using ProductManagement.Application.Common;
using ProductManagement.Application.DTOs.Product;
using ProductManagement.Application.Interfaces.Repositories;
using ProductManagement.Application.Interfaces.Services;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace ProductManagement.Application.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateProductDto> _createValidator;
    private readonly IValidator<UpdateProductDto> _updateValidator;
    private readonly ILogger<ProductService> _logger;

    public ProductService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<CreateProductDto> createValidator,
    IValidator<UpdateProductDto> updateValidator,
    ILogger<ProductService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    public async Task<PagedResult<ProductDto>> GetAllAsync(int pageNumber,int pageSize)
    {
        var products = await _unitOfWork.Products.GetAllAsync(pageNumber, pageSize);

        var totalCount = await _unitOfWork.Products.CountAsync();

        var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

        return new PagedResult<ProductDto>
        {
            Items = productDtos,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<ProductDto> GetByIdAsync(int id)
    {
        var product = await _unitOfWork.Products
            .GetByIdAsync(id);

        if (product == null)
        {
            throw new NotFoundException(
                $"Product with Id {id} was not found.");
        }

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> CreateAsync(
    CreateProductDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "Validation failed while creating product. Errors: {@Errors}",
                validationResult.Errors.Select(x => x.ErrorMessage));

            throw new AppValidationException(
                validationResult.Errors.Select(x => x.ErrorMessage));
        }

        var product = _mapper.Map<Product>(dto);

        product.CreatedBy = "System";

        product.CreatedOn = DateTime.UtcNow;

        await _unitOfWork.Products
            .AddAsync(product);

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Product created successfully. ProductId: {ProductId}, ProductName: {ProductName}",product.Id,product.ProductName);

        return _mapper.Map<ProductDto>(product);
    }

    public async Task UpdateAsync(
    int id,
    UpdateProductDto dto)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning(
                "Validation failed while updating product. Errors: {@Errors}",
                validationResult.Errors.Select(x => x.ErrorMessage));

            throw new AppValidationException(
                validationResult.Errors.Select(x => x.ErrorMessage));
        }

        var product = await _unitOfWork.Products
            .GetByIdAsync(id);

        if (product == null)
        {
            _logger.LogWarning(
                "Product not found. ProductId: {ProductId}",
                id);

            throw new NotFoundException(
                $"Product with Id {id} was not found.");
        }

        _mapper.Map(dto, product);

        product.ModifiedBy = "System";

        product.ModifiedOn = DateTime.UtcNow;

        _unitOfWork.Products.Update(product);

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Product updated successfully. ProductId: {ProductId}",product.Id);


    }

    public async Task DeleteAsync(int id)
    {
        var product = await _unitOfWork.Products
            .GetByIdAsync(id);

        

        _unitOfWork.Products.Delete(product);

        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Product deleted successfully. ProductId: {ProductId}",product.Id);
    }
}