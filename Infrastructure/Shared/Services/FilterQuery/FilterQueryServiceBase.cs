using Domain.Shared.Entities;
using Domain.Shared.Interfaces;
using Domain.Shared.Queries;
using Infrastructure.Shared.Specifications.DataSource;
using Infrastructure.Shared.Specifications.Filter;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Shared.Services.FilterQuery;

public abstract class FilterQueryServiceBase<TDomain> : IFilterQueryService<TDomain>
    where TDomain : ModelBase
{
    protected readonly DataContext _context;

    protected FilterQueryServiceBase(DataContext context)
    {
        _context = context;
    }

    protected abstract IDataSourceSpecification<TDomain> DataSource { get; }

    protected abstract IFilterSpecification<TDomain> FilterSpecification { get; }

    public virtual async Task<List<TDomain>> GetPaginatedListAsync(IQueryParameter<TDomain> param)
    {
        var (page, limit) = (param.Page, param.Limit);

        var query = FilterSpecification.GetFilteredQuery(DataSource.Source, param);
        var result = page != null && limit != null
            ? await query
                .Skip(((int)page - 1) * (int)limit)
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
