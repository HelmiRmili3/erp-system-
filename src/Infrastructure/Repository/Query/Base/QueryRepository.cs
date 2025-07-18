using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Backend.Application.Common.Interfaces;
using Backend.Infrastructure.Data;
using Backend.Application.Common.Parameters;
using Backend.Application.Common.Response;

namespace Backend.Infrastructure.Repository.Query.Base;

public class QueryRepository<T>(ApplicationDbContext dbContext) : IQueryRepository<T> where T : class
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<T>().FindAsync(id, cancellationToken);
    }

    public virtual async Task<T?> GetByIdWithIncludeAsync(Expression<Func<T, bool>> filter,
                                                          string? includeTable,
                                                          CancellationToken cancellationToken)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        if (!string.IsNullOrEmpty(includeTable))
        {
            foreach (var table in includeTable.Split([','], StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(table);
            }
        }

        return await query.FirstOrDefaultAsync(filter, cancellationToken);
    }

    public async Task<List<T>> GetAllByFilterAsync(Expression<Func<T, bool>> filter,
                                                   string? includeTable = "",
                                                   CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbContext.Set<T>();
        if (!string.IsNullOrEmpty(includeTable))
        {
            foreach (var table in includeTable.Split([','], StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(table);
            }
        }

        return await query.Where(filter).ToListAsync(cancellationToken: cancellationToken);
    }

    public virtual async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Set<T>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task<List<T>> GetAllWithIncludeAsync(string? includeTable, CancellationToken cancellationToken)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        if (!string.IsNullOrEmpty(includeTable))
        {
            foreach (var table in includeTable.Split([','], StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(table);
            }
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken)
    {
        IQueryable<T> query = _dbContext.Set<T>();
        return await query.Where(filter).AnyAsync(cancellationToken);
    }

    public async Task<T?> GetSingleByFilterAsync(Expression<Func<T, bool>> filter,
                                                 string? includeTable = "",
                                                 CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbContext.Set<T>();
        if (!string.IsNullOrEmpty(includeTable))
        {
            foreach (var table in includeTable.Split([','], StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(table);
            }
        }

        return await query.FirstOrDefaultAsync(filter, cancellationToken);
    }
    public async Task<PagedResponse<List<T>>> GetPagedAsync(
     Expression<Func<T, bool>>? filter,
     int pageNumber,
     int pageSize,
     string? includeTable,
     CancellationToken cancellationToken)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        if (!string.IsNullOrWhiteSpace(includeTable))
        {
            query = query.Include(includeTable);
        }

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        int totalRecords = await _dbContext.Set<T>().CountAsync(cancellationToken); // total without filter
        int filteredRecords = await query.CountAsync(cancellationToken); // total with filter

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResponse<List<T>>(
            items,
            pageNumber,
            pageSize,
            new RecordsCount
            {
                RecordsFiltered = filteredRecords,
                RecordsTotal = totalRecords
            }
        );
    }



}
