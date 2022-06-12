using System.Linq.Expressions;

namespace Infrastructure.Shared;

public static class QuerySearchExtension
{
    public static ICollection<QuerySearchExpression<T>> AddExpression<T>(
        this ICollection<QuerySearchExpression<T>> expressionsNode,
        Expression<Func<T, bool>> expression,
        CombineMode combineMode,
        Guid blockId
    )
    {
        expressionsNode.Add(new QuerySearchExpression<T>(expression, combineMode, blockId));
        return expressionsNode;
    }

    public static IQueryable<T> ApplyExpressionsNode<T>(
        this IQueryable<T> query,
        ICollection<QuerySearchExpression<T>> expressionsNode
    )
    {
        var group = expressionsNode.GroupBy(g => g.BlockId);

        foreach (var item in group)
        {
            CombineMode combineMode = item.Select(x => x.CombineMode).First();
            var expressions = item.Select(x => x.Expression);

            if (combineMode == CombineMode.OrElse)
                query = query.Where(expressions.OrElse());
            else
                query = query.Where(expressions.And());
        }

        return query;
    }
}
