using System.Linq.Expressions;

namespace Infrastructure.Shared;

public class QuerySearchExpression<T>
{
    public Expression<Func<T, bool>> Expression { get; set; }
    public CombineMode CombineMode { get; set; }
    public Guid BlockId { get; set; }

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