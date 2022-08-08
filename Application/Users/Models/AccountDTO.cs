using System.ComponentModel.DataAnnotations;

namespace Application.Users.Models;

public class TokenModel
{
    public string Token { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}

public class LoginModel
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class RegisterModel
{
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
}
