namespace Backend.Application.Common.Extensions;
public abstract class Specification<T>
{
    public virtual IQueryable<T> ApplyFilter(IQueryable<T> query)
    {
        return query;
    }
}

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, Specification<T> specification)
    {
        return specification.ApplyFilter(query);
    }
}
