using Microsoft.AspNetCore.Mvc;
using ProductManagement.Application.Common;
using ProductManagement.Application.Common.Models;
using ProductManagement.Application.DTOs.Product;
using ProductManagement.Application.Interfaces.Services;

namespace ProductManagement.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(
        IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
    int pageNumber = 1,
    int pageSize = 10)
    {
        var result = await _productService
            .GetAllAsync(pageNumber, pageSize);

        return Ok(
        ApiResponse<PagedResult<ProductDto>>
        .SuccessResponse(
            result,
            "Products retrieved successfully."));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService
            .GetByIdAsync(id);

        return Ok(
        ApiResponse<ProductDto>
        .SuccessResponse(
            product,
            "Product retrieved successfully."));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody]
    CreateProductDto dto)
    {
        var product = await _productService
            .CreateAsync(dto);

        return CreatedAtAction(
        nameof(GetById),
        new { id = product.Id },
        ApiResponse<ProductDto>
        .SuccessResponse(
            product,
            "Product created successfully.",
            201));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
    int id,
    [FromBody] UpdateProductDto dto)
    {
        await _productService.UpdateAsync(id, dto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteAsync(id);

        return NoContent();
    }

}