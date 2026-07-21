using System.Linq.Expressions;

namespace ProductManagement.Application.Interfaces.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize);

    Task<int> CountAsync();

    Task<T?> GetByIdAsync(int id);

    Task AddAsync(T entity);

    void Update(T entity);

    void Delete(T entity);

    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
}