using Domain.Interfaces;

namespace Infrastructure.DataModels;

public class DataModelBase : IEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
    public string? OwnerUserId { get; set; }
    public UserDataModel? OwnerUser { get; set; }
}
