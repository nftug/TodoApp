using Domain.Users;

namespace Application.Users;

public class UserCommandDTO
{
    [UserEmailAttribute]
    public string? Username { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
}
