using Client.Services.Authentication;
using Domain.Users.Entities;
using Microsoft.AspNetCore.Components;

namespace Client.Pages;

public partial class SignUpPage : ComponentBase
{
    [Inject]
    protected IAuthService AuthService { get; set; } = null!;
    [Inject]
    protected NavigationManager Navigation { get; set; } = null!;

    [SupplyParameterFromQuery]
    [Parameter]
    public string? Redirect { get; set; }

    public RegisterCommand _registerCommand = new();
    public bool _isLoading = false;

    public async Task SubmitAsync()
    {
        _isLoading = true;
        var result = await AuthService.RegisterAsync(_registerCommand);

        if (result.IsSuccessful)
            Navigation.NavigateTo(Redirect ?? "/");

        _isLoading = false;
    }
}