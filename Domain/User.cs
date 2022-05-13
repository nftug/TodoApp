using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class ApplicationUser : IdentityUser
    {
    }

    public class UserModel
    {
        public string? Token { get; set; }
        public string? Username { get; set; }
        public string? Id { get; set; }
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
        [RegularExpression(@"^[a-zA-Z0-9.?/-]{8,24}$", ErrorMessage = "Password must be complex")]
        public string? Password { get; set; }
        [Required]
        public string? Username { get; set; }
    }
}