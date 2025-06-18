using Microsoft.EntityFrameworkCore;
using Backend.Application.Common.Interfaces;
using Backend.Infrastructure.Data;

namespace Backend.Infrastructure.Repository.Command.Base;

public class CommandRepository<T>(ApplicationDbContext context) : ICommandRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context = context;

    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
    {
        await _context.Set<T>().AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
