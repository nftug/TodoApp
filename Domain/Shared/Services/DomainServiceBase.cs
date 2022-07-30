using Domain.Services;
using Domain.Shared.Entities;
using Domain.Shared.Interfaces;
using Domain.Shared.Queries;

namespace Domain.Shared.Services;

public abstract class DomainServiceBase<TDomain> : IDomainService<TDomain>
    where TDomain : ModelBase
{
    protected readonly IRepository<TDomain> _repository;

    public DomainServiceBase(IRepository<TDomain> repository)
    {
        _repository = repository;
    }

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
