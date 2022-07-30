using Domain.Shared.Entities;

namespace Application.Shared.Interfaces;

public interface ICommandDTO<TDomain>
    where TDomain : ModelBase
{
    Guid? Id { get; init; }
}
