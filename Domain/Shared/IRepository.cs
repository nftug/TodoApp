namespace Domain.Shared;

public interface IRepository<T, TDataModel>
{
    Task<T> CreateAsync(T T);
    Task<T> UpdateAsync(T T);
    Task<T?> FindAsync(Guid id);
    Task RemoveAsync(Guid id);
    Task<List<T>> GetPaginatedListAsync
        (IQueryable<TDataModel> query, IQueryParameter param);
}
