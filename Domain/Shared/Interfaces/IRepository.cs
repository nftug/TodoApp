using Domain.Shared.Entities;

namespace Domain.Shared.Interfaces;

public interface IRepository<TDomain>
    where TDomain : ModelBase
{
    Task<TDomain> CreateAsync(TDomain item);

    Task<TDomain?> UpdateAsync(TDomain item);

    Task<TDomain?> FindAsync(Guid id);

    Task RemoveAsync(Guid id);
}
