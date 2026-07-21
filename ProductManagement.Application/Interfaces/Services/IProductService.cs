using ProductManagement.Application.Common;
using ProductManagement.Application.DTOs.Product;

namespace ProductManagement.Application.Interfaces.Services;

public interface IProductService
{
    Task<PagedResult<ProductDto>> GetAllAsync(int pageNumber, int pageSize);

    Task<ProductDto> GetByIdAsync(int id);

    Task<ProductDto> CreateAsync(CreateProductDto dto);

    Task UpdateAsync(int id, UpdateProductDto dto);

    Task DeleteAsync(int id);
}