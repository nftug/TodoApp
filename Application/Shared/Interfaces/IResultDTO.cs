using Domain.Shared;

namespace Application.Shared.Interfaces;

public interface IResultDTO<TDomain>
    where TDomain : ModelBase
{
    Guid Id { get; }
    DateTime CreatedDateTime { get; }
    DateTime UpdatedDateTime { get; }
    string? OwnerUserId { get; }
}
