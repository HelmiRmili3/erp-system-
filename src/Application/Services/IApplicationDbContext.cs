using Backend.Domain.Entities;

namespace Backend.Application.Services;

public interface IApplicationDbContext
{

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    Task<bool> ItemExists<T>(int id, CancellationToken cancellationToken) where T : class;
}
