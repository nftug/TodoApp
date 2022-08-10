using System.Net.Http.Json;
using Application.Users.Models;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Client.Services.Authentication;

public class AuthService : IAuthService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly HttpClient _httpClient;
    protected readonly ISnackbar _snackbar;

    public AuthService(
        AuthenticationStateProvider authenticationStateProvider,
        HttpClient httpClient,
        ISnackbar snackbar
    )
    {
        _authenticationStateProvider = authenticationStateProvider;
        _httpClient = httpClient;
        _snackbar = snackbar;
    }

    public async Task<LoginResult> LoginAsync(LoginModel loginModel)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("auth/login", loginModel);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<TokenModel>();
            if (result == null) throw new HttpRequestException();

            var loginResult = new LoginResult
            {
                IsSuccessful = true,
                Result = result
            };

            await ((SpaAuthenticateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(result);
            _snackbar.Add("ログインしました。", Severity.Info);

            return loginResult;
        }
        catch (HttpRequestException e)
        {
            _snackbar.Add("ログインに失敗しました。", Severity.Error);

            return new LoginResult
            {
                IsSuccessful = false,
                Error = e
            };
        }
    }

    public async Task LogoutAsync()
    {
        await ((SpaAuthenticateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        _snackbar.Add("ログアウトしました。", Severity.Info);
    }
}
