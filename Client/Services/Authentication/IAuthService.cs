using Domain.Users.Entities;

namespace Client.Services.Authentication;

public interface IAuthService
{
    Task<LoginResult> LoginAsync(LoginCommand loginModel);
    Task LogoutAsync();
}

public class LoginResult
{
    public bool IsSuccessful { get; set; }
    public Exception Error { get; set; } = null!;
    public TokenModel Result { get; set; } = null!;
}