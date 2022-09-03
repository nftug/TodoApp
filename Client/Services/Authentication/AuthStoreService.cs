using System.Security.Claims;

namespace Client.Services.Authentication;

public class AuthStoreService
{
    public event Action? Notify;

    private ClaimsPrincipal? _user;
    public ClaimsPrincipal? User
    {
        get => _user;
        set
        {
            if (value == _user) return;
            _user = value;
            Notify?.Invoke();
        }
    }

    public Guid UserId
    {
        get
        {
            var idString = User?.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return idString != null ? Guid.Parse(idString) : new Guid();
        }
    }

    public string? UserName => User?.Identity?.Name;

    public bool IsLoggedIn => User?.Identity?.Name != null;
}
