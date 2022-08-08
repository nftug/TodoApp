using System.Net.Http.Json;
using Application.Users.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace Client.Services.Authentication;

public class AuthService : IAuthService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly HttpClient _httpClient;

    public AuthService(
        AuthenticationStateProvider authenticationStateProvider,
        HttpClient httpClient
    )
    {
        _authenticationStateProvider = authenticationStateProvider;
        _httpClient = httpClient;
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
            return loginResult;
        }
        catch (HttpRequestException e)
        {
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
    }
}
