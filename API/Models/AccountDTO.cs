using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class TokenModel
{
    public string? Token { get; set; }
}

public class LoginModel
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class RegisterModel
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
    [Required]
    public string? Username { get; set; }
}
