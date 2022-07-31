using Domain.Users.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.DataModels;

public class UserDataModel<T> : IdentityUser<Guid>, IDataModel<User>
    where T : struct
{
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public Guid? OwnerUserId { get; set; }

    public UserDataModel() { }

    public UserDataModel(User origin)
    {
        Transfer(origin);
    }

    public void Transfer(User origin)
    {
        Id = origin.Id;
        CreatedOn = origin.CreatedOn;
        UpdatedOn = origin.UpdatedOn;
        OwnerUserId = origin.OwnerUserId;

        UserName = origin.UserName.Value;
        Email = origin.Email.Value;
    }
}
