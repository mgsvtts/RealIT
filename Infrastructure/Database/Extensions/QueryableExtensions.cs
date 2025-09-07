using System.Linq.Expressions;

namespace Infrastructure.Database.Extensions;


public static class QueryableExtensions
{
    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> query,
        bool? @if,
        Expression<Func<T, bool>> then,
        Expression<Func<T, bool>>? @else = null)
    {
        if (@if == true)
        {
            return query.Where(then);
        }

        if (@else is not null)
        {
            return query.Where(@else);
        }

        return query;
    }
}