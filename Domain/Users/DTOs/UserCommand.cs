using Domain.Shared.DTOs;
using Domain.Users.Entities;
using Domain.Users.ValueObjects;

namespace Domain.Users.DTOs;

public class UserCommand : ICommand<User>
{
    public Guid? Id { get; set; }
    [UserName]
    public string? UserName { get; set; } = string.Empty;
    [UserEmail]
    public string? Email { get; set; } = string.Empty;

    public UserCommand() { }

    public UserCommand(UserResultDTO.Me origin)
    {
        Id = origin.Id;
        UserName = origin.UserName;
        Email = origin.Email;
    }
}
