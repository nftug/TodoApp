using Domain.Interfaces;
using Domain.Shared;

namespace Infrastructure.DataModels;

public class DataModelBase<TDomain> : IEntity<TDomain>
    where TDomain : ModelBase
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public Guid? OwnerUserId { get; set; }
    public UserDataModel<Guid>? OwnerUser { get; set; }
}
