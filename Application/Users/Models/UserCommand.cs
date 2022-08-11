using Application.Shared.Interfaces;
using Domain.Users.Entities;
using Domain.Users.ValueObjects;

namespace Application.Users.Models;

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