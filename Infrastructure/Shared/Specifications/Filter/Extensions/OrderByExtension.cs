using System.Linq.Expressions;

namespace Infrastructure.Shared.Specifications.Filter.Extensions;

internal static class OrderByExtension
{
    public static IOrderedQueryable<TSource> OrderByAscDesc<TSource, TKey>(
        this IQueryable<TSource> query,
        Expression<Func<TSource, TKey>> keySelector,
        bool isDescending = false
    ) => isDescending
        ? query.OrderByDescending(keySelector)
        : query.OrderBy(keySelector);
}
