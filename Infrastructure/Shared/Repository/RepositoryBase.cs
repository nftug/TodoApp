using Domain.Shared;
using Infrastructure.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Shared.Repository;

public abstract class RepositoryBase<T, TDataModel> : IRepository<T, TDataModel>
    where T : ModelBase
    where TDataModel : DataModelBase
{
    protected readonly DataContext _context;

    public RepositoryBase(DataContext context)
    {
        _context = context;
    }

    public virtual async Task<T> CreateAsync(T item)
    {
        var data = ToDataModel(item);
        await _context.Set<TDataModel>().AddAsync(data);
        await _context.SaveChangesAsync();

        return ToModel(data);
    }

    public virtual async Task<T> UpdateAsync(T item)
    {
        var foundData = await _context
            .Set<TDataModel>()
            .FirstOrDefaultAsync(x => x.Id == item.Id);

        if (foundData == null)
            throw new NotFoundException();

        var data = Transfer(item, foundData);

        _context.Set<TDataModel>().Update(data);
        await _context.SaveChangesAsync();

        return ToModel(data);
    }

    public virtual async Task<T?> FindAsync(Guid id)
    {
        var data = await _context
            .Set<TDataModel>()
            .FirstOrDefaultAsync(x => x.Id == id);

        return data != null ? ToModel(data) : null;
    }

    public virtual async Task<List<T>> GetPaginatedListAsync
        (IQueryable<TDataModel> query, IQueryParameter param)
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
        var TDataModel = await _context.Set<TDataModel>().FindAsync(id);

        if (TDataModel == null)
            throw new NotFoundException();

        _context.Set<TDataModel>().Remove(TDataModel);
        await _context.SaveChangesAsync();
    }

    protected abstract TDataModel ToDataModel(T item);

    protected abstract TDataModel Transfer(T item, TDataModel data);

    protected abstract T ToModel(TDataModel data);
}
