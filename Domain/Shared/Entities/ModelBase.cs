namespace Domain.Shared.Entities;

public abstract class ModelBase
{
    public Guid Id { get; }
    public DateTime CreatedOn { get; }
    public DateTime UpdatedOn { get; private set; }
    public Guid? OwnerUserId { get; }

    // データモデルからの変換用
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

    // 新規作成用
    protected ModelBase(Guid ownerUserId) : this()
    {
        OwnerUserId = ownerUserId;
    }

    protected ModelBase()
    {
        var operationDateTime = DateTime.Now;
        CreatedOn = operationDateTime;
        UpdatedOn = operationDateTime;
    }

    protected void SetUpdatedOn()
    {
        UpdatedOn = DateTime.Now;
    }
}
