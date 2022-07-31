using System.Linq.Expressions;
using Infrastructure.Shared.Specifications.Filter.Models;

namespace Infrastructure.Shared.Specifications.Filter.Extensions;

internal static class QueryFilterExtension
{
    public static void AddSearch<T>(
        this ICollection<ExpressionGroup<T>> expressionGroups,
        string? fieldValue,
        Func<Keyword, Expression<Func<T, bool>>> expressionFunc
    )
    {
        var searchField = new SearchField<T>(fieldValue)
        {
            Node = Keyword
                .CreateFromRawString(fieldValue)
                .Select(x => new QueryFilterExpression<T>
                {
                    Expression = expressionFunc(x),
                    CombineMode = x.CombineMode,
                    BlockId = x.Id,
                })
        };

        expressionGroups.AddSearchField(searchField);
    }

    public static void AddSimpleSearch<T>(
        this ICollection<ExpressionGroup<T>> expressionGroups,
        object? fieldValue,
        Expression<Func<T, bool>> expression
    )
    {
        var searchField = new SearchField<T>
        {
            Node = Enumerable
                .Range(0, fieldValue != null ? 1 : 0)
                .Select(x => new QueryFilterExpression<T> { Expression = expression })
        };

        expressionGroups.AddSearchField(searchField);
    }

    public static IQueryable<T> ApplyExpressionGroup<T>(
        this IQueryable<T> query,
        IEnumerable<ExpressionGroup<T>> nodes
    )
    {
        if (!nodes.Any()) return query;

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

        return query.Where(expression);
    }

    private static void AddSearchField<T>(
        this ICollection<ExpressionGroup<T>> expressionGroups,
        SearchField<T> field
    )
    {
        if (!field.Node.Any()) return;

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

        expressionGroups.Add(
            new ExpressionGroup<T>
            {
                CombineMode = field.CombineMode,
                Expression = expression
            }
        );
    }
}
