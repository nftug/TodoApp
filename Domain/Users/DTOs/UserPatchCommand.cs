using Domain.Shared.DTOs;
using Domain.Users.Entities;
using Domain.Users.ValueObjects;

namespace Domain.Users.DTOs;

public class UserPatchCommand : ICommand<User>
{
    public Guid? Id { get; set; }
    [UserName(isPatch: true)]
    public string? UserName { get; set; }
    [UserEmail(isPatch: true)]
    public string? Email { get; set; }

    public UserPatchCommand() { }

    public UserPatchCommand(UserResultDTO.Me origin)
    {
        Id = origin.Id;
        UserName = origin.UserName;
        Email = origin.Email;
    }
}