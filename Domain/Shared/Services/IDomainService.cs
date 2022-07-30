using Domain.Shared.Entities;
using Domain.Shared.Queries;

namespace Domain.Services;

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
