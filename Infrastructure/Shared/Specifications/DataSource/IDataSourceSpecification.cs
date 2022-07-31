using Domain.Shared.Entities;
using Infrastructure.DataModels;

namespace Infrastructure.Shared.Specifications.DataSource;

public interface IDataSourceSpecification<TDomain>
    where TDomain : ModelBase
{
    IQueryable<IDataModel<TDomain>> Source { get; }

    TDomain ToDomain(IDataModel<TDomain> origin, bool recursive = false);
}
