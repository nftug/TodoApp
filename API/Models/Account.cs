using System.ComponentModel.DataAnnotations;

namespace API.Models
{
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
        [RegularExpression(@"^[a-zA-Z0-9.?/-]{8,24}$", ErrorMessage = "Password must be complex")]
        public string? Password { get; set; }
        [Required]
        public string? Username { get; set; }
    }

    public class UserModel
    {
        public class Private
        {
            public string? Id { get; set; }
            public string? Username { get; set; }
            public string? Email { get; set; }
        }
        public class Public
        {
            public string? Id { get; set; }
            public string? Username { get; set; }
        }
    }
}