using Domain.Shared;

namespace Domain.Interfaces;

public interface IDomainService<TDomain>
    where TDomain : ModelBase
{
    Task<bool> CanCreate(TDomain item, Guid? userId);
    Task<bool> CanEdit(TDomain item, Guid? userId);
    Task<bool> CanDelete(TDomain item, Guid? userId);
    Task<bool> CanShow(TDomain item, Guid? userId);
    IQueryParameter<TDomain> GetQueryParameter(
        IQueryParameter<TDomain> param,
        Guid? userId
    );
}
