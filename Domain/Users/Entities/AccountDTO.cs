using System.ComponentModel.DataAnnotations;
using Domain.Users.ValueObjects;

namespace Domain.Users.Entities;

public class TokenModel
{
    public string Token { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public Guid UserId { get; set; }
}

public class LoginCommand
{
    [UserEmail]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "パスワードを入力してください。")]
    [StringLength(32, ErrorMessage = "パスワードが長すぎます。")]
    public string Password { get; set; } = string.Empty;
}

public class RegisterCommand
{
    [UserEmail]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "パスワードを入力してください。")]
    [StringLength(32, ErrorMessage = "パスワードが長すぎます。")]
    public string Password { get; set; } = string.Empty;
    [UserName]
    public string UserName { get; set; } = string.Empty;
}
