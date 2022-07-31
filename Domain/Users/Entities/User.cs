using Domain.Shared.Entities;
using Domain.Users.ValueObjects;

namespace Domain.Users.Entities;

public class User : ModelBase
{
    public UserName UserName { get; private set; } = null!;
    public UserEmail Email { get; private set; } = null!;

    public User(
        Guid id,
        DateTime createdOn,
        DateTime updatedOn,
        Guid? ownerUserId,
        UserName userName,
        UserEmail email
    )
        : base(id, createdOn, updatedOn, ownerUserId)
    {
        UserName = userName;
        Email = email;
    }

    private User() { }

    public void Edit(
        UserName userName,
        UserEmail email
    )
    {
        UserName = userName;
        Email = email;
    }
}
