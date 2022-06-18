using System.Linq.Expressions;
using Infrastructure.Shared.QuerySearch.Models;

namespace Infrastructure.Shared.QuerySearch.Extensions;

internal static class QuerySearchExtension
{
    public static void AddExpression<T>(
        this ICollection<QuerySearchExpression<T>> expressionsNode,
        Expression<Func<T, bool>> expression,
        Keyword keyword
    )
    {
        expressionsNode.Add(
            new QuerySearchExpression<T>(
                expression,
                keyword.CombineMode,
                keyword.Id
            )
        );
        return;
    }

    public static IQueryable<T> ApplyExpressionsNode<T>(
        this IQueryable<T> query,
        ICollection<QuerySearchExpression<T>> expressionsNode
    )
    {
        var expressions = expressionsNode
            .GroupBy(x => new
            { x.BlockId, x.CombineMode }, (k, g) => new
            {
                k.CombineMode,
                Expressions = g.Select(x => x.Expression)
            })
            .Select(x =>
                x.CombineMode == CombineMode.OrElse
                    ? x.Expressions.OrElse()
                    : x.Expressions.And()
            );

        foreach (var expression in expressions)
            query = query.Where(expression);

        return query;
    }
}
