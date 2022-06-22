using Domain.Shared;

namespace Application.Shared.Interfaces;

public interface IResultDTO<TDomain>
    where TDomain : ModelBase
{
    Guid Id { get; }
    DateTime CreatedOn { get; }
    DateTime UpdatedOn { get; }
    Guid? OwnerUserId { get; }
}
