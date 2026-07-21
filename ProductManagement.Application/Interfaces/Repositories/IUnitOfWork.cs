using ProductManagement.Domain.Entities;

namespace ProductManagement.Application.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }

    IItemRepository Items { get; }

    Task<int> SaveChangesAsync();
}