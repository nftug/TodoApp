using System.Linq.Expressions;

namespace Infrastructure.Services.QueryService.Models;

public class QueryFilterExpression<T>
{
    public Expression<Func<T, bool>> Expression { get; init; } = null!;
    public CombineMode CombineMode { get; init; }
    public Guid BlockId { get; init; }
}

public class ExpressionGroup<T>
{
    public CombineMode CombineMode { get; init; }
    public Expression<Func<T, bool>> Expression { get; set; } = null!;
}