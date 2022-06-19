using System.Linq.Expressions;

namespace Infrastructure.Shared.QuerySearch.Models;

public class QuerySearchExpression<T>
{
    public Expression<Func<T, bool>> Expression { get; init; } = null!;
    public CombineMode CombineMode { get; init; }
    public Guid BlockId { get; init; }
}