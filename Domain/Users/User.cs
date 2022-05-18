namespace Domain.Users;

public class User
{
    public string Id { get; set; }
    public UserUserName UserName { get; private set; }
    public UserEmail Email { get; private set; }

    public User(
        string id,
        UserUserName userName,
        UserEmail email
    )
    {
        Id = id;
        UserName = userName;
        Email = email;
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
