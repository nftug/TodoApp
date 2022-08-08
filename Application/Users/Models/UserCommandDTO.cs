using Application.Shared.Interfaces;
using Domain.Users.Entities;
using Domain.Users.ValueObjects;

namespace Application.Users.Models;

public class UserCommandDTO : ICommandDTO<User>
{
    public Guid? Id { get; set; }
    [UserName]
    public string? Username { get; set; } = string.Empty;
    [UserEmail]
    public string? Email { get; set; } = string.Empty;
}
