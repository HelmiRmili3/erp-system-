namespace Backend.Application.Common.Interfaces;
public interface ICommandRepository<T> where T : class
{
    Task<T> AddAsync(T entity, CancellationToken cancellationToken);

    Task UpdateAsync(T entity, CancellationToken cancellationToken);

    Task DeleteAsync(T entity, CancellationToken cancellationToken);
}
