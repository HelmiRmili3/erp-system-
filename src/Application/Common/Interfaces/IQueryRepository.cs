using System.Linq.Expressions;
using Backend.Application.Common.Response;

namespace Backend.Application.Common.Interfaces;

public interface IQueryRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<T?> GetByIdWithIncludeAsync(Expression<Func<T, bool>> filter, string? includeTable, CancellationToken cancellationToken);
    Task<List<T>> GetAllByFilterAsync(Expression<Func<T, bool>> filter, string? includeTable, CancellationToken cancellationToken);
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken);
    Task<bool> ExistAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
    Task<List<T>> GetAllWithIncludeAsync(string? includeTable, CancellationToken cancellationToken);
    Task<T?> GetSingleByFilterAsync(Expression<Func<T, bool>> filter, string? includeTable, CancellationToken cancellationToken);
    Task<PagedResponse<List<T>>> GetPagedAsync(
    Expression<Func<T, bool>>? filter,
    int pageNumber,
    int pageSize,
    string? includeTable,
    CancellationToken cancellationToken);

}
