using Domain.Interfaces;

namespace Infrastructure.Interfaces;

public interface IQuerySearch<TEntity>
    where TEntity : IEntity
{
    IQueryable<TEntity> GetFilteredQuery(IQueryParameter<TEntity> param);
}
