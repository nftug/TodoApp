using Application.Shared.Interfaces;
using Domain.User;

namespace Application.User;

public class UserCommandDTO : ICommandDTO<UserModel>
{
    public Guid? Id { get; init; }
    [UserName]
    public string? Username { get; set; } = string.Empty;
    [UserEmail]
    public string? Email { get; set; } = string.Empty;
}
