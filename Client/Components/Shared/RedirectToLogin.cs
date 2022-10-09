using Microsoft.AspNetCore.Components;

namespace Client.Components.Shared;

public class RedirectToLogin : ComponentBase
{
    [Inject]
    private NavigationManager Navigation { get; set; } = null!;

    protected override void OnInitialized()
    {
        var currentUri = Navigation.ToBaseRelativePath(Navigation.Uri);
        Navigation.NavigateTo($"/login?redirect={currentUri}", false, true);
    }
}
