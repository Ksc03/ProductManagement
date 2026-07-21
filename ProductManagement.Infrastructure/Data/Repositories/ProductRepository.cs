using ProductManagement.Application.Interfaces.Repositories;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Infrastructure.Data.Repositories;

public class ProductRepository
    : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    // Product-specific queries will go here in the future.
}