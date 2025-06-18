using Backend.Domain.Entities;

namespace Backend.Application.Services;

public interface IApplicationDbContext
{
    //DbSet<Category> Categories { get; }
    //DbSet<Configuration> Configurations { get; }
    DbSet<Employee> Employees { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    Task<bool> ItemExists<T>(int id, CancellationToken cancellationToken) where T : class;
}
