using Domain.Shared.Entities;

namespace Domain.Shared.Queries;

public interface IQueryParameter<TDomain>
    where TDomain : ModelBase
{
    int? Page { get; init; }
    int? Limit { get; init; }
    string Sort { get; init; }
}
