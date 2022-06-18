namespace Domain.Shared;

public abstract class ModelBase
{
    public Guid Id { get; set; }
    public DateTime CreatedDateTime { get; protected set; }
    public DateTime UpdatedDateTime { get; protected set; }
    public string? OwnerUserId { get; protected set; }
}
