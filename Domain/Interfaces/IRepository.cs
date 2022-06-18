using Domain.Shared;

namespace Domain.Interfaces;

public interface IRepository<T, TEntity>
    where T : ModelBase
    where TEntity : IEntity
{
    Task<T> CreateAsync(T item);
    Task<T> UpdateAsync(T item);
    Task<T?> FindAsync(Guid id);
    Task RemoveAsync(Guid id);
    Task<List<T>> GetPaginatedListAsync
        (IQueryable<TEntity> query, IQueryParameter<TEntity> param);
}
