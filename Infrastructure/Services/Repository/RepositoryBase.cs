using AutoMapper;
using Domain.Interfaces;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Repository;

public abstract class RepositoryBase<TDomain> : IRepository<TDomain>
    where TDomain : ModelBase
{
    protected readonly DataContext _context;
    protected readonly IMapper _mapper;

    public RepositoryBase
        (DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public virtual async Task<TDomain> CreateAsync(TDomain item)
    {
        var data = MapToEntity(item);
        await AddEntityAsync(data);
        await _context.SaveChangesAsync();

        return MapToDomain(data);
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
        return await DomainSource
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
            .Select(x => MapToDomain(x))
            .ToList();
    }

    public virtual async Task RemoveAsync(Guid id)
    {
        var data = await Source
            .FirstOrDefaultAsync(x => x.Id == id);

        if (data == null)
            throw new NotFoundException();

        RemoveEntity(data);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<List<TDomain>> GetListAsync
        (IQueryable<IEntity<TDomain>> query)
    {
        return (await query.ToListAsync())
            .Select(x => MapToDomain(x))
            .ToList();
    }

    protected abstract IQueryable<IEntity<TDomain>> Source { get; }

    protected abstract IEntity<TDomain> MapToEntity(TDomain item);

    protected virtual TDomain MapToDomain(IEntity<TDomain> entity)
        => _mapper.Map<TDomain>(entity);

    protected virtual void Transfer(TDomain item, IEntity<TDomain> entity)
        => _mapper.Map(item, entity);

    protected abstract Task AddEntityAsync(IEntity<TDomain> entity);

    protected abstract void UpdateEntity(IEntity<TDomain> entity);

    protected abstract void RemoveEntity(IEntity<TDomain> entity);

    protected virtual IQueryable<TDomain> DomainSource
        => _mapper.ProjectTo<TDomain>(Source);
}
