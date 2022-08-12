// Reference: https://stackoverflow.com/a/7265354

using System.Linq.Expressions;

namespace Infrastructure.Shared.Specifications.Filter.Extensions;

internal static class OrderByExtension
{
    public static IQueryable<TSource> OrderByKey<TSource>(
        this IQueryable<TSource> source,
        string? key,
        bool isDescending
    )
    {
        if (string.IsNullOrEmpty(key)) return source;

        var command = isDescending ? "OrderByDescending" : "OrderBy";
        var property = typeof(TSource).GetProperty(key);
        if (property == null) throw new InvalidOperationException();

        var parameter = Expression.Parameter(typeof(TSource), "x");
        var propertyAccess = Expression.MakeMemberAccess(parameter, property!);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);
        var resultExpression = Expression
            .Call(typeof(Queryable), command, new Type[] { typeof(TSource), property.PropertyType },
                    source.Expression, Expression.Quote(orderByExpression));

        return source.Provider.CreateQuery<TSource>(resultExpression);
    }
}
