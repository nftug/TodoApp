using Domain.Shared.Entities;
using Domain.Shared.Exceptions;
using Domain.Shared.Interfaces;
using Infrastructure.DataModels;
using Infrastructure.Shared.Specifications.DataSource;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Shared.Services.Repository;

public abstract class RepositoryBase<TDomain> : IRepository<TDomain>
    where TDomain : ModelBase
{
    protected readonly DataContext _context;

    protected abstract IDataSourceSpecification<TDomain> DataSource { get; }

    public RepositoryBase(DataContext context)
    {
        _context = context;
    }

    public virtual async Task<TDomain> CreateAsync(TDomain item)
    {
        var data = ToDataModel(item);
        await AddEntityAsync(data);
        await _context.SaveChangesAsync();

        return DataSource.ToDomain(data);
    }

    public virtual async Task<TDomain?> UpdateAsync(TDomain item)
    {
        var data = await DataSource.Source.FirstOrDefaultAsync(x => x.Id == item.Id);

        if (data == null)
            throw new NotFoundException();

        Transfer(item, data);

        UpdateEntity(data);
        await _context.SaveChangesAsync();

        return await FindAsync(data.Id);
    }

    public virtual async Task<TDomain?> FindAsync(Guid id)
    {
        var result = await DataSource.Source.FirstOrDefaultAsync(x => x.Id == id);
        return result != null ? DataSource.ToDomain(result) : null; ;
    }

    public virtual async Task RemoveAsync(Guid id)
    {
        var data = await DataSource.Source.FirstOrDefaultAsync(x => x.Id == id);

        if (data == null)
            throw new NotFoundException();

        RemoveEntity(data);
        await _context.SaveChangesAsync();
    }

    protected abstract IDataModel<TDomain> ToDataModel(TDomain origin);

    protected abstract void Transfer(TDomain origin, IDataModel<TDomain> dataModel);

    protected abstract Task AddEntityAsync(IDataModel<TDomain> entity);

    protected abstract void UpdateEntity(IDataModel<TDomain> entity);

    protected abstract void RemoveEntity(IDataModel<TDomain> entity);
}
