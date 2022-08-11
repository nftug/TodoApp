using Application.Shared.Interfaces;
using Domain.Shared.Entities;
using Domain.Shared.Queries;

namespace Application.Shared.UseCases;

public class Queries<TDomain, TResultDTO>
    where TDomain : ModelBase
    where TResultDTO : IResultDTO<TDomain>
{
    public virtual DetailsBase<TDomain, TResultDTO>.Query Details(Guid id, Guid? userId)
        => new(id, userId);
    public virtual ListBase<TDomain, TResultDTO>.Query List(IQueryParameter<TDomain> param, Guid? userId)
        => new(param, userId);
}