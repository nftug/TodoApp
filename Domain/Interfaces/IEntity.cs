using Domain.Shared;

namespace Domain.Interfaces;

public interface IEntity<TDomain>
    where TDomain : ModelBase
{
    Guid Id { get; set; }
    DateTime CreatedDateTime { get; set; }
    DateTime UpdatedDateTime { get; set; }
    string? OwnerUserId { get; set; }
    // UserDataModel? OwnerUser { get; set; }
}
