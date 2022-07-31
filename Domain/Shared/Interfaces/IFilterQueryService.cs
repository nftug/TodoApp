using Domain.Shared.Entities;
using Domain.Shared.Queries;

namespace Domain.Shared.Interfaces;

public interface IFilterQueryService<TDomain>
    where TDomain : ModelBase
{
    Task<List<TDomain>> GetPaginatedListAsync(IQueryParameter<TDomain> param);

    Task<List<TDomain>> GetListAsync(IQueryParameter<TDomain> param);

    Task<int> GetCountAsync(IQueryParameter<TDomain> param);
}
