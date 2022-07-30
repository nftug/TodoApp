using Domain.Shared.Entities;
using Infrastructure.DataModels;

namespace Domain.Interfaces;

public interface IQueryService<TDomain>
    where TDomain : ModelBase
{
    IQueryable<IDataModel<TDomain>> GetFilteredQuery(
        IQueryable<IDataModel<TDomain>> source,
        IQueryParameter<TDomain> param
    );
}
