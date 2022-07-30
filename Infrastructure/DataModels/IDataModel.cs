using Domain.Shared.Entities;

namespace Infrastructure.DataModels;

public interface IDataModel<TDomain>
    where TDomain : ModelBase
{
    Guid Id { get; set; }
    DateTime CreatedOn { get; set; }
    DateTime UpdatedOn { get; set; }
    Guid? OwnerUserId { get; set; }
}
