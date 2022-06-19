using AutoMapper;
using Domain.Interfaces;
using Domain.Shared;
using Infrastructure.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Shared.Repository;

public abstract class RepositoryBase<TDomain, TEntity> : IRepository<TDomain, TEntity>
    where TDomain : ModelBase
    where TEntity : DataModelBase
{
    protected readonly DataContext _context;
    protected readonly IMapper _mapper;

    public RepositoryBase(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public virtual async Task<TDomain> CreateAsync(TDomain item)
    {
        var data = _mapper.Map<TEntity>(item);
        await _context.Set<TEntity>().AddAsync(data);
        await _context.SaveChangesAsync();

        return _mapper.Map<TDomain>(data);
    }

    public virtual async Task<TDomain> UpdateAsync(TDomain item)
    {
        var data = await _context
            .Set<TEntity>()
            .FirstOrDefaultAsync(x => x.Id == item.Id);

        if (data == null)
            throw new NotFoundException();

        _mapper.Map(item, data);

        _context.Set<TEntity>().Update(data);
        await _context.SaveChangesAsync();

        return _mapper.Map<TDomain>(data);
    }

    public virtual async Task<TDomain?> FindAsync(Guid id)
    {
        var data = await _context
            .Set<TEntity>()
            .FirstOrDefaultAsync(x => x.Id == id);

        return data != null ? _mapper.Map<TDomain>(data) : null;
    }

    public virtual async Task<List<TDomain>> GetPaginatedListAsync
        (IQueryable<TEntity> query, IQueryParameter<TEntity> param)
    {
        var (page, limit) = (param.Page, param.Limit);

        return (await query
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync())
            .Select(x => _mapper.Map<TDomain>(x))
            .ToList();
    }

    public virtual async Task RemoveAsync(Guid id)
    {
        var data = await _context.Set<TEntity>().FindAsync(id);

        if (data == null)
            throw new NotFoundException();

        _context.Set<TEntity>().Remove(data);
        await _context.SaveChangesAsync();
    }
}
