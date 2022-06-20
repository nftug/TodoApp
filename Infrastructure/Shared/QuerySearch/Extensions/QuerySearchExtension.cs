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
            new QuerySearchExpression<T>()
            {
                Expression = expression,
                CombineMode = keyword.CombineMode,
                BlockId = keyword.Id,
            }
        );
        return;
    }

    public static void AddExpressionNode<T>(
        this ICollection<ExpressionGroup<T>> nodes,
        SearchField<T> field
    )
    {
        if (field.Node.Count == 0) return;

        var expression = field.Node
            .GroupBy(x => new
            { x.BlockId, x.CombineMode }, (k, g) => new
            {
                k.CombineMode,
                Expressions = g.Select(x => x.Expression)
            })
            .Select(x =>
                x.CombineMode == CombineMode.OrElse
                    ? x.Expressions.OrElse()
                    : x.Expressions.And())
            .And();

        nodes.Add(
            new ExpressionGroup<T>()
            {
                CombineMode = field.CombineMode,
                Expression = expression
            }
        );
    }

    public static IQueryable<T> ApplyExpressionGroup<T>(
        this IQueryable<T> query,
        ICollection<ExpressionGroup<T>> nodes
    )
    {
        var expression = nodes
            .GroupBy(x => new
            { x.CombineMode }, (k, g) => new
            {
                k.CombineMode,
                Expressions = g.Select(x => x.Expression)
            })
            .Select(x =>
                x.CombineMode == CombineMode.OrElse
                    ? x.Expressions.OrElse()
                    : x.Expressions.And())
            .And();

        query = query.Where(expression);

        return query;
    }
}
