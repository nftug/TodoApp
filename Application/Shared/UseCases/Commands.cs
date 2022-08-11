using Application.Shared.Interfaces;
using Domain.Shared.Entities;

namespace Application.Shared.UseCases;

public class Commands<TDomain, TResultDTO, TCommand, TPatchCommand>
    where TDomain : ModelBase
    where TResultDTO : IResultDTO<TDomain>
    where TCommand : ICommand<TDomain>
    where TPatchCommand : ICommand<TDomain>
{
    public virtual CreateBase<TDomain, TResultDTO, TCommand>.Command Create(TCommand item, Guid userId)
        => new(item, userId);
    public virtual EditBase<TDomain, TResultDTO, TCommand>.Command Put(Guid id, TCommand item, Guid userId)
        => new(id, item, userId);
    public virtual EditBase<TDomain, TResultDTO, TPatchCommand>.Command Patch(Guid id, TPatchCommand item, Guid userId)
        => new(id, item, userId);
    public virtual DeleteBase<TDomain>.Command Delete(Guid id, Guid userId)
        => new(id, userId);
}
