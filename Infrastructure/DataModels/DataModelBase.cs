using Domain.Interfaces;
using Domain.Shared;

namespace Infrastructure.DataModels;

public class DataModelBase<TDomain> : IEntity<TDomain>
    where TDomain : ModelBase
{
    public Guid Id { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
    public string? OwnerUserId { get; set; }
    public UserDataModel? OwnerUser { get; set; }
}
