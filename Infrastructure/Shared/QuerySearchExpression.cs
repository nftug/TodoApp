using System.Linq.Expressions;

namespace Infrastructure.Shared;

public class QuerySearchExpression<T>
{
    public Expression<Func<T, bool>> Expression { get; }
    public CombineMode CombineMode { get; }
    public Guid BlockId { get; }

    public QuerySearchExpression(
        Expression<Func<T, bool>> expression,
        CombineMode combineMode,
        Guid blockId
    )
    {
        Expression = expression;
        CombineMode = combineMode;
        BlockId = blockId;
    }
}