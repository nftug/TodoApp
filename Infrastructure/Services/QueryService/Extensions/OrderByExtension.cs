using System.Linq.Expressions;

namespace Infrastructure.Services.QueryService.Extensions;

public static class OrderByExtension
{
    public static IOrderedQueryable<TSource> OrderByAscDesc<TSource, TKey>(
        this IQueryable<TSource> query,
        Expression<Func<TSource, TKey>> keySelector,
        bool isDescending = false
    ) => isDescending
        ? query.OrderByDescending(keySelector)
        : query.OrderBy(keySelector);
}
