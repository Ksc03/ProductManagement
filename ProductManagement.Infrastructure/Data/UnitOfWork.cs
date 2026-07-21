using ProductManagement.Application.Interfaces.Repositories;
using ProductManagement.Infrastructure.Data.Repositories;

namespace ProductManagement.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IProductRepository Products { get; }

    public IItemRepository Items { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;

        Products = new ProductRepository(_context);
        Items = new ItemRepository(_context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}