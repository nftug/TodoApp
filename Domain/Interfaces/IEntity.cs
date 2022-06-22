using Domain.Shared;

namespace Domain.Interfaces;

public interface IEntity<TDomain>
    where TDomain : ModelBase
{
    Guid Id { get; set; }
    DateTime CreatedOn { get; set; }
    DateTime UpdatedOn { get; set; }
    Guid? OwnerUserId { get; set; }
}
