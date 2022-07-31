using System.Linq.Expressions;

namespace Infrastructure.Shared.Specifications.Filter.Models;

internal class QueryFilterExpression<T>
{
    public Expression<Func<T, bool>> Expression { get; init; } = null!;
    public CombineMode CombineMode { get; init; }
    public Guid BlockId { get; init; }
}

internal class ExpressionGroup<T>
{
    public CombineMode CombineMode { get; init; }
    public Expression<Func<T, bool>> Expression { get; set; } = null!;
}