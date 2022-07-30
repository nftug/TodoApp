using Domain.Interfaces;
using Domain.Shared.Entities;
using Domain.Shared.Exceptions;
using Infrastructure.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repository;

public abstract class RepositoryBase<TDomain> : IRepository<TDomain>
    where TDomain : ModelBase
{
    protected readonly DataContext _context;
    protected readonly IQueryService<TDomain> _queryService;

    public RepositoryBase
        (DataContext context, IQueryService<TDomain> queryService)
    {
        _context = context;
        _queryService = queryService;
    }

    internal RepositoryBase()
    {
        _context = null!;
        _queryService = null!;
    }

    public virtual async Task<TDomain> CreateAsync(TDomain item)
    {
        var data = ToDataModel(item);
        await AddEntityAsync(data);
        await _context.SaveChangesAsync();

        return ToDomain(data);
    }

    public virtual async Task<TDomain?> UpdateAsync(TDomain item)
    {
        var data = await Source
            .FirstOrDefaultAsync(x => x.Id == item.Id);

        if (data == null)
            throw new NotFoundException();

        Transfer(item, data);

        UpdateEntity(data);
        await _context.SaveChangesAsync();

        return await FindAsync(data.Id);
    }

    public virtual async Task<TDomain?> FindAsync(Guid id)
    {
        var result = await Source.FirstOrDefaultAsync(x => x.Id == id);
        return result != null ? ToDomain(result) : null; ;
    }

    public virtual async Task<List<TDomain>> GetPaginatedListAsync(IQueryParameter<TDomain> param)
    {
        var (page, limit) = (param.Page, param.Limit);

        var query = _queryService.GetFilteredQuery(Source, param);
        var result = page != null && limit != null
            ? await query
                .Skip(((int)page - 1) * (int)limit)
                .Take((int)limit)
                .ToListAsync()
            : await query.ToListAsync();

        return result
            .Select(x => ToDomain(x))
            .ToList();
    }

    public virtual async Task<int> GetCountAsync(IQueryParameter<TDomain> param)
        => await _queryService.GetFilteredQuery(Source, param).CountAsync();

    public virtual async Task RemoveAsync(Guid id)
    {
        var data = await Source
            .FirstOrDefaultAsync(x => x.Id == id);

        if (data == null)
            throw new NotFoundException();

        RemoveEntity(data);
        await _context.SaveChangesAsync();
    }

    protected abstract IQueryable<IDataModel<TDomain>> Source { get; }

    protected abstract IDataModel<TDomain> ToDataModel(TDomain origin);

    internal abstract TDomain ToDomain(IDataModel<TDomain> origin, bool recursive = false);

    protected abstract void Transfer(TDomain origin, IDataModel<TDomain> dataModel);

    protected abstract Task AddEntityAsync(IDataModel<TDomain> entity);

    protected abstract void UpdateEntity(IDataModel<TDomain> entity);

    protected abstract void RemoveEntity(IDataModel<TDomain> entity);
}
