using Domain.Shared;

namespace Domain.Interfaces;

public interface IQueryService<TDomain>
    where TDomain : ModelBase
{
    IQueryable<IEntity<TDomain>> GetFilteredQuery(IQueryParameter<TDomain> param);
}
