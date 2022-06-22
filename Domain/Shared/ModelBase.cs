namespace Domain.Shared;

public abstract class ModelBase
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; protected set; }
    public DateTime UpdatedOn { get; protected set; }
    public Guid? OwnerUserId { get; protected set; }
}
