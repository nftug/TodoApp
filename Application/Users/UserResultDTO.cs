using Domain.Users;

namespace Application.Users;

public class UserResultDTO
{
    public class Me
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public Me(string id, string username, string email)
        {
            Id = id;
            Username = username;
            Email = email;
        }

        public static Me CreateResultDTO(User user) =>
            new Me(user.Id, user.UserName.Value, user.Email.Value);
    }
    public class Public
    {
        public string Id { get; set; }
        public string Username { get; set; }

        public Public(string id, string username)
        {
            Id = id;
            Username = username;
        }

        public static Public CreateResultDTO(User user) =>
            new Public(user.Id, user.UserName.Value);
    }
}
