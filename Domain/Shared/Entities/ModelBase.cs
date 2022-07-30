namespace Domain.Shared.Entities;

public abstract class ModelBase
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; protected set; }
    public DateTime UpdatedOn { get; protected set; }
    public Guid? OwnerUserId { get; protected set; }

    protected ModelBase(
        Guid id,
        DateTime createdOn,
        DateTime updatedOn,
        Guid? ownerUserId
    )
    {
        Id = id;
        CreatedOn = createdOn;
        UpdatedOn = updatedOn;
        OwnerUserId = ownerUserId;
    }

    protected ModelBase() { }
}
