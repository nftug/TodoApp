using Domain.Shared.Entities;
using Infrastructure.DataModels;

namespace Infrastructure.Shared.Specifications.DataSource;

internal abstract class DataSourceSpecificationBase<TDomain> : IDataSourceSpecification<TDomain>
    where TDomain : ModelBase
{
    protected readonly DataContext _context;

    public DataSourceSpecificationBase(DataContext context)
    {
        _context = context;
    }

    public abstract IQueryable<IDataModel<TDomain>> Source { get; }

    public abstract TDomain ToDomain(IDataModel<TDomain> origin, bool recursive = false);
}
