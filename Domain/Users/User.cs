namespace Domain.Users;

public class User
{
    public string Id { get; set; }
    public UserUserName UserName { get; private set; }
    public UserEmail Email { get; private set; }

    private User(
        string id,
        UserUserName userName,
        UserEmail email
    )
    {
        Id = id;
        UserName = userName;
        Email = email;
    }

    public static User CreateFromRepository(
        string id,
        UserUserName userName,
        UserEmail email
    )
    {
        return new User(
            id: id,
            userName: userName,
            email: email
        );
    }

    public void Edit(
        UserUserName userName,
        UserEmail email
    )
    {
        UserName = userName;
        Email = email;
    }
}
