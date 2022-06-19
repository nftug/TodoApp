using Domain.Shared;

namespace Domain.Interfaces;

public interface IRepository<TDomain, TEntity>
    where TDomain : ModelBase
    where TEntity : IEntity
{
    Task<TDomain> CreateAsync(TDomain item);
    Task<TDomain> UpdateAsync(TDomain item);
    Task<TDomain?> FindAsync(Guid id);
    Task RemoveAsync(Guid id);
    Task<List<TDomain>> GetPaginatedListAsync
        (IQueryable<TEntity> query, IQueryParameter<TEntity> param);
}
