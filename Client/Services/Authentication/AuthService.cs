using System.Net.Http.Json;
using Domain.Users.Entities;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Client.Services.Authentication;

public class AuthService : IAuthService
{
    private readonly SpaAuthenticateProvider _authenticationStateProvider;
    private readonly HttpClient _httpClient;
    protected readonly ISnackbar _snackbar;

    public AuthService(
        AuthenticationStateProvider authenticationStateProvider,
        HttpClient httpClient,
        ISnackbar snackbar
    )
    {
        _authenticationStateProvider = (SpaAuthenticateProvider)authenticationStateProvider;
        _httpClient = httpClient;
        _snackbar = snackbar;
    }

    public async Task<LoginResult> LoginAsync(LoginCommand command)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("auth/login", command);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<TokenModel>();
            if (result == null) throw new HttpRequestException();

            var loginResult = new LoginResult
            {
                IsSuccessful = true,
                Result = result
            };

            await _authenticationStateProvider.MarkUserAsAuthenticated(result);
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
        await _authenticationStateProvider.MarkUserAsLoggedOut();
        _snackbar.Add("ログアウトしました。", Severity.Info);
    }

    public async Task<LoginResult> RegisterAsync(RegisterCommand command)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("auth/register", command);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<TokenModel>();
            if (result == null) throw new HttpRequestException();

            var loginResult = new LoginResult
            {
                IsSuccessful = true,
                Result = result
            };

            await _authenticationStateProvider.MarkUserAsAuthenticated(result);
            _snackbar.Add("ユーザー登録が完了しました。", Severity.Info);

            return loginResult;
        }
        catch (HttpRequestException e)
        {
            _snackbar.Add("ユーザー登録に失敗しました。", Severity.Error);

            return new LoginResult
            {
                IsSuccessful = false,
                Error = e
            };
        }
    }
}
