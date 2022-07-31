namespace Domain.Shared.Entities;

public abstract class ModelBase
{
    public Guid Id { get; }
    public DateTime CreatedOn { get; }
    public DateTime UpdatedOn { get; private set; }
    public Guid? OwnerUserId { get; }

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

    protected ModelBase(DateTime createdOn, Guid? ownerUserId)
    {
        CreatedOn = createdOn;
        UpdatedOn = createdOn;
        OwnerUserId = ownerUserId;
    }

    protected void SetUpdatedOn()
    {
        UpdatedOn = DateTime.Now;
    }
}
