using System.ComponentModel.DataAnnotations;
using Application.Users.Models;
using Client.Models;
using Client.Services.Authentication;
using Domain.Users.ValueObjects;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Client.Pages;

public partial class LoginPage : MyComponentBase
{
    [Inject]
    protected IAuthService AuthService { get; set; } = null!;
    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

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
        {
            Snackbar.Add("ログインしました。", Severity.Info);
            Navigation.NavigateTo(Redirect ?? "/");
        }
        else
        {
            Snackbar.Add("ログインに失敗しました。", Severity.Error);
        }

        IsLoading = false;
    }
}

public class LoginCommand
{
    [UserEmail]
    // [Required(ErrorMessage = "メールアドレスを入力してください。")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "パスワードを入力してください。")]
    [StringLength(32, ErrorMessage = "パスワードが長すぎます。")]
    public string Password { get; set; } = string.Empty;
}