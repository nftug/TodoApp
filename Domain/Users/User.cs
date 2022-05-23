namespace Domain.Users;

public class User
{
    public string Id { get; set; }
    public UserName UserName { get; private set; }
    public UserEmail Email { get; private set; }

    public User(
        string id,
        UserName userName,
        UserEmail email
    )
    {
        Id = id;
        UserName = userName;
        Email = email;
    }

    public void Edit(
        UserName userName,
        UserEmail email
    )
    {
        UserName = userName;
        Email = email;
    }
}
