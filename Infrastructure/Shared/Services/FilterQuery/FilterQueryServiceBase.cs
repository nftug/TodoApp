using Domain.Shared.Entities;
using Domain.Shared.Interfaces;
using Domain.Shared.Queries;
using Infrastructure.DataModels;
using Infrastructure.Shared.Specifications.DataSource;
using Infrastructure.Shared.Specifications.Filter;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Shared.Services.FilterQuery;

public abstract class FilterQueryServiceBase<TDomain, TDataModel> : IFilterQueryService<TDomain>
    where TDomain : ModelBase
    where TDataModel : IDataModel<TDomain>
{
    protected readonly DataContext _context;

    protected FilterQueryServiceBase(DataContext context)
    {
        _context = context;
    }

    protected abstract IDataSourceSpecification<TDomain> DataSource { get; }

    protected abstract IFilterSpecification<TDomain, TDataModel> FilterSpecification { get; }

    public virtual async Task<List<TDomain>> GetPaginatedListAsync(IQueryParameter<TDomain> param)
    {
        var (startIndex, limit) = (param.StartIndex, param.Limit);

        var query = FilterSpecification.GetFilteredQuery(DataSource.Source, param);
        var result = startIndex != null && limit != null
            ? await query
                .Skip((int)startIndex)
                .Take((int)limit)
                .ToListAsync()
            : await query.ToListAsync();

        return result
            .Select(x => DataSource.ToDomain(x))
            .ToList();
    }

    public virtual async Task<List<TDomain>> GetListAsync(IQueryParameter<TDomain> param)
    {
        var query = FilterSpecification.GetFilteredQuery(DataSource.Source, param);
        return (await query.ToListAsync())
            .Select(x => DataSource.ToDomain(x))
            .ToList();
    }

    public virtual async Task<int> GetCountAsync(IQueryParameter<TDomain> param)
        => await FilterSpecification.GetFilteredQuery(DataSource.Source, param).CountAsync();
}
