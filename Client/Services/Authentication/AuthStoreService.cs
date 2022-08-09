using System.Security.Claims;

namespace Client.Services.Authentication;

public class AuthStoreService
{
    public ClaimsPrincipal? User { get; private set; }

    public Guid UserId
    {
        get
        {
            var idString = User?.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return idString != null ? Guid.Parse(idString) : new Guid();
        }
    }

    public string? UserName => User?.Identity?.Name ?? "ゲスト";

    public bool IsLoggedIn => User?.Identity?.Name != null;

    public void SetUser(ClaimsPrincipal? user)
    {
        User = user;
    }
}
