using Domain.Shared;

namespace Domain.Interfaces;

public interface IQueryParameter<TDomain>
    where TDomain : ModelBase
{
    int Page { get; init; }
    int Limit { get; init; }
}
