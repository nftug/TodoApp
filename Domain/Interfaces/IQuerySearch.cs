using Domain.Shared;

namespace Domain.Interfaces;

public interface IQuerySearch<TDomain>
    where TDomain : ModelBase
{
    IQueryable<IEntity<TDomain>> GetFilteredQuery(IQueryParameter<TDomain> param);
}
