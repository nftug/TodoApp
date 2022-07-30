using Domain.Shared.Entities;
using Domain.Users.ValueObjects;

namespace Domain.Users.Entities;

public class User : ModelBase
{
    public UserName UserName { get; private set; } = null!;
    public UserEmail Email { get; private set; } = null!;

    public void Edit(
        UserName userName,
        UserEmail email
    )
    {
        UserName = userName;
        Email = email;
    }
}
