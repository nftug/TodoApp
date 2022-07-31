using System.Linq.Expressions;
using Infrastructure.Shared.Specifications.Filter.Models;

namespace Infrastructure.Shared.Specifications.Filter.Extensions;

internal static class QueryFilterExtension
{
    public static void AddExpression<T>(
        this ICollection<QueryFilterExpression<T>> expressionsNode,
        Keyword keyword,
        Expression<Func<T, bool>> expression
    )
    {
        expressionsNode.Add(
            new QueryFilterExpression<T>
            {
                Expression = expression,
                CombineMode = keyword.CombineMode,
                BlockId = keyword.Id,
            }
        );
    }

    public static void AddExpression<T>(
        this ICollection<QueryFilterExpression<T>> expressionsNode,
        Expression<Func<T, bool>> expression
    )
    {
        expressionsNode.Add(new QueryFilterExpression<T> { Expression = expression });
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
            new ExpressionGroup<T>
            {
                CombineMode = field.CombineMode,
                Expression = expression
            }
        );
    }

    public static void AddSearch<T>(
        this ICollection<ExpressionGroup<T>> expressionGroups,
        string? fieldValue,
        Action<List<QueryFilterExpression<T>>, Keyword> addExpressionFunc
    )
    {
        var searchField = new SearchField<T>(fieldValue);

        foreach (var keyword in Keyword.CreateFromRawString(fieldValue))
            addExpressionFunc(searchField.Node, keyword);

        expressionGroups.AddExpressionNode(searchField);
    }

    public static void AddSimpleSearch<T>(
        this ICollection<ExpressionGroup<T>> expressionGroups,
        object? fieldValue,
        Expression<Func<T, bool>> expression
    )
    {
        var searchField = new SearchField<T>();

        if (fieldValue != null)
            searchField.Node.AddExpression(expression);

        expressionGroups.AddExpressionNode(searchField);
    }

    public static IQueryable<T> ApplyExpressionGroup<T>(
        this IQueryable<T> query,
        ICollection<ExpressionGroup<T>> nodes
    )
    {
        if (nodes.Count == 0) return query;

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
