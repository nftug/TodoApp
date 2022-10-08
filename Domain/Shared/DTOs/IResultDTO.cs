using Domain.Shared.Entities;

namespace Domain.Shared.DTOs;

public interface IResultDTO<TDomain>
    where TDomain : ModelBase
{
    Guid Id { get; }
    DateTime CreatedOn { get; }
    DateTime UpdatedOn { get; }
    Guid? OwnerUserId { get; }
}
