using Domain.Shared.Entities;
using Domain.Shared.Queries;
using Infrastructure.DataModels;

namespace Infrastructure.Shared.Specifications.Filter;

public interface IFilterSpecification<TDomain, TDataModel>
    where TDomain : ModelBase
    where TDataModel : IDataModel<TDomain>
{
    IQueryable<IDataModel<TDomain>> GetFilteredQuery(
        IQueryable<IDataModel<TDomain>> source,
        IQueryParameter<TDomain> param
    );
}
