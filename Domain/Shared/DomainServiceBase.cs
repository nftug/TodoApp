using Domain.Interfaces;

namespace Domain.Shared;

public class DomainServiceBase<TDomain> : IDomainService<TDomain>
    where TDomain : ModelBase
{
    public virtual Task<bool> CanDelete(TDomain item, Guid? userId)
        => Task.FromResult(item.OwnerUserId == userId);

    public virtual Task<bool> CanEdit(TDomain item, Guid? userId)
        => Task.FromResult(item.OwnerUserId == userId);

    public virtual Task<bool> CanCreate(TDomain item, Guid? userId)
        => Task.FromResult(true);

    public virtual Task<bool> CanShow(TDomain item, Guid? userId)
        => Task.FromResult(true);

    public virtual IQueryParameter<TDomain> GetQueryParameter(
        IQueryParameter<TDomain> param,
        Guid? userId
    )
        => param;
}
