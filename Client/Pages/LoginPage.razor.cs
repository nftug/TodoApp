using System.ComponentModel.DataAnnotations;
using Client.Services.Authentication;
using Domain.Users.Entities;
using Domain.Users.ValueObjects;
using Microsoft.AspNetCore.Components;

namespace Client.Pages;

public partial class LoginPage : ComponentBase
{
    [Inject]
    protected IAuthService AuthService { get; set; } = null!;
    [Inject]
    protected NavigationManager Navigation { get; set; } = null!;

    [SupplyParameterFromQuery]
    [Parameter]
    public string? Redirect { get; set; }

    public LoginModel LoginModel { get; set; } = new();
    public bool _isLoading = false;

    public async Task SubmitAsync()
    {
        _isLoading = true;

        var model = new LoginCommand
        {
            Email = LoginModel.Email!,
            Password = LoginModel.Password
        };
        var result = await AuthService.LoginAsync(model);

        if (result.IsSuccessful)
            Navigation.NavigateTo(Redirect ?? "/");

        _isLoading = false;
    }
}

public class LoginModel
{
    [UserEmail]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "パスワードを入力してください。")]
    [StringLength(32, ErrorMessage = "パスワードが長すぎます。")]
    public string Password { get; set; } = string.Empty;
}