using Domain.Interfaces;
using Domain.User;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.DataModels;

public class UserDataModel<T> : IdentityUser<Guid>, IEntity<UserModel>
    where T : struct
{
    public DateTime CreatedDateTime { get; set; }
    public DateTime UpdatedDateTime { get; set; }
    public Guid? OwnerUserId { get; set; }
}
