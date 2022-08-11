using Domain.Shared.Entities;

namespace Infrastructure.DataModels;

public class DataModelBase<TDomain> : IDataModel<TDomain>
    where TDomain : ModelBase
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public Guid? OwnerUserId { get; set; }
    public UserDataModel? OwnerUser { get; set; }

    public virtual void Transfer(ModelBase origin)
    {
        Id = origin.Id;
        CreatedOn = origin.CreatedOn;
        UpdatedOn = origin.UpdatedOn;
        OwnerUserId = origin.OwnerUserId;
    }
}
