using Domain.Interfaces;
using Domain.Shared;
using Infrastructure.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Shared.Repository;

public abstract class RepositoryBase<T, TEntity> : IRepository<T, TEntity>
    where T : ModelBase
    where TEntity : DataModelBase
{
    protected readonly DataContext _context;

    public RepositoryBase(DataContext context)
    {
        _context = context;
    }

    public virtual async Task<T> CreateAsync(T item)
    {
        var data = ToDataModel(item);
        await _context.Set<TEntity>().AddAsync(data);
        await _context.SaveChangesAsync();

        return ToModel(data);
    }

    public virtual async Task<T> UpdateAsync(T item)
    {
        var foundData = await _context
            .Set<TEntity>()
            .FirstOrDefaultAsync(x => x.Id == item.Id);

        if (foundData == null)
            throw new NotFoundException();

        var data = Transfer(item, foundData);

        _context.Set<TEntity>().Update(data);
        await _context.SaveChangesAsync();

        return ToModel(data);
    }

    public virtual async Task<T?> FindAsync(Guid id)
    {
        var data = await _context
            .Set<TEntity>()
            .FirstOrDefaultAsync(x => x.Id == id);

        return data != null ? ToModel(data) : null;
    }

    public virtual async Task<List<T>> GetPaginatedListAsync
        (IQueryable<TEntity> query, IQueryParameter<TEntity> param)
    {
        var (page, limit) = (param.Page, param.Limit);

        return (await query
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync())
            .Select(x => ToModel(x))
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

    protected abstract TEntity ToDataModel(T item);

    protected abstract TEntity Transfer(T item, TEntity data);

    protected abstract T ToModel(TEntity data);
}
