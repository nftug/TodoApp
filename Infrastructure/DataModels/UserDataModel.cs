using Domain.Interfaces;
using Domain.User;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.DataModels;

public class UserDataModel<T> : IdentityUser<Guid>, IEntity<UserModel>
    where T : struct
{
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public Guid? OwnerUserId { get; set; }
}
