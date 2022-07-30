using Domain.Shared.Entities;
using Domain.Shared.Queries;
using Infrastructure.DataModels;

namespace Domain.Shared.Interfaces;

public interface IQueryService<TDomain>
    where TDomain : ModelBase
{
    IQueryable<IDataModel<TDomain>> GetFilteredQuery(
        IQueryable<IDataModel<TDomain>> source,
        IQueryParameter<TDomain> param
    );
}
