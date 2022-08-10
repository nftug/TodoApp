using System.ComponentModel.DataAnnotations;
using Application.Users.Models;
using Client.Models;
using Client.Services.Authentication;
using Domain.Users.ValueObjects;
using Microsoft.AspNetCore.Components;

namespace Client.Pages;

public partial class LoginPage : MyComponentBase
{
    [Inject]
    protected IAuthService AuthService { get; set; } = null!;

    [SupplyParameterFromQuery]
    [Parameter]
    public string? Redirect { get; set; }

    public LoginCommand LoginCommand { get; set; } = new();
    public bool IsLoading { get; set; }

    public async Task SubmitAsync()
    {
        IsLoading = true;

        var model = new LoginModel
        {
            Email = LoginCommand.Email!,
            Password = LoginCommand.Password
        };
        var result = await AuthService.LoginAsync(model);

        if (result.IsSuccessful)
            Navigation.NavigateTo(Redirect ?? "/");

        IsLoading = false;
    }
}

public class LoginCommand
{
    [UserEmail]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "パスワードを入力してください。")]
    [StringLength(32, ErrorMessage = "パスワードが長すぎます。")]
    public string Password { get; set; } = string.Empty;
}