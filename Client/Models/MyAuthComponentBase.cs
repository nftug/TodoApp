using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Client.Models;

public class MyAuthComponentBase : MyComponentBase
{
    [CascadingParameter]
    protected Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

    protected ClaimsPrincipal User { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        User = (await AuthenticationStateTask).User;
        await base.OnInitializedAsync();
    }

    protected Guid UserId
    {
        get
        {
            var idString = User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return idString != null ? Guid.Parse(idString) : new Guid();
        }
    }

    protected string? UserName => User.Identity?.Name;
}
