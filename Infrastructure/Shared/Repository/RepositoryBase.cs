using Domain.Interfaces;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Shared.Repository;

public abstract class RepositoryBase<TDomain> : IRepository<TDomain>
    where TDomain : ModelBase
{
    protected readonly DataContext _context;
    protected readonly IDataSource<TDomain> _source;

    public RepositoryBase
        (DataContext context, IDataSource<TDomain> source)
    {
        _context = context;
        _source = source;
    }

    public virtual async Task<TDomain> CreateAsync(TDomain item)
    {
        var data = _source.MapToEntity(item);
        await _source.AddEntityAsync(data);
        await _context.SaveChangesAsync();

        return _source.MapToDomain(data);
    }

    public virtual async Task<TDomain?> UpdateAsync(TDomain item)
    {
        var data = await _source.Source
            .FirstOrDefaultAsync(x => x.Id == item.Id);

        if (data == null)
            throw new NotFoundException();

        _source.Transfer(item, data);

        _source.UpdateEntity(data);
        await _context.SaveChangesAsync();

        return await FindAsync(data.Id);
    }

    public virtual async Task<TDomain?> FindAsync(Guid id)
    {
        return await _source
            .DomainSource
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public virtual async Task<List<TDomain>> GetPaginatedListAsync
        (IQueryable<IEntity<TDomain>> query, IQueryParameter<TDomain> param)
    {
        var (page, limit) = (param.Page, param.Limit);

        return (await query
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync())
            .Select(x => _source.MapToDomain(x))
            .ToList();
    }

    public virtual async Task RemoveAsync(Guid id)
    {
        var data = await _source.Source
            .FirstOrDefaultAsync(x => x.Id == id);

        if (data == null)
            throw new NotFoundException();

        _source.RemoveEntity(data);
        await _context.SaveChangesAsync();
    }
}
