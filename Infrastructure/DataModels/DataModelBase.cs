namespace Infrastructure.DataModels;

public class DataModelBase
{
    public Guid Id { get; set; }
    public DateTime CreatedDateTime { get; internal set; }
    public DateTime UpdatedDateTime { get; internal set; }
    public string? OwnerUserId { get; internal set; }
    public UserDataModel? OwnerUser { get; set; }
}
