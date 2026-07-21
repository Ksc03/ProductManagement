using AutoMapper;
using ProductManagement.Application.DTOs.Product;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Application.Mapping;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductDto, Product>();

        CreateMap<UpdateProductDto, Product>();

        CreateMap<Product, ProductDto>();
    }
}