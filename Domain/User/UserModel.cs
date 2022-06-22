using Domain.Shared;

namespace Domain.User;

public class UserModel : ModelBase
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
