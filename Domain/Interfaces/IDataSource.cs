using Domain.Shared;

namespace Domain.Interfaces;

public interface IDataSource<TDomain>
    where TDomain : ModelBase
{
    IQueryable<IEntity<TDomain>> Source { get; }
    TDomain MapToDomain(IEntity<TDomain> entity);
    IEntity<TDomain> MapToEntity(TDomain item);
    void Transfer(TDomain item, IEntity<TDomain> entity);

    Task AddEntityAsync(IEntity<TDomain> entity);
    void UpdateEntity(IEntity<TDomain> entity);
    void RemoveEntity(IEntity<TDomain> entity);
}
