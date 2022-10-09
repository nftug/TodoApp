using Domain.Shared.Entities;

namespace Domain.Shared.Queries;

public interface IQueryParameter<TDomain>
    where TDomain : ModelBase
{
    int? Page { get; set; }
    int? StartIndex { get; set; }
    int? Limit { get; set; }
    string Sort { get; set; }
}
