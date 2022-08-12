using System.Net.Http.Headers;
using System.Security.Claims;
using Blazored.LocalStorage;
using Domain.Users.Entities;
using Microsoft.AspNetCore.Components.Authorization;

namespace Client.Services.Authentication;

public class SpaAuthenticateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private const string Key = "tokenModel";

    public SpaAuthenticateProvider(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var tokenModel = await _localStorage.GetItemAsync<TokenModel>(Key);

        // トークンが見つからなければ未ログイン扱いにする
        if (tokenModel == null)
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        // HttpClientのヘッダーにトークンを加える
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", tokenModel.Token);

        var claims = new List<Claim> {
            new Claim(ClaimTypes.Name, tokenModel.UserName),
            new Claim(ClaimTypes.NameIdentifier, tokenModel.UserId.ToString())
        };

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "User"));
        return new AuthenticationState(user);
    }

    public async Task MarkUserAsAuthenticated(TokenModel tokenModel)
    {
        await _localStorage.SetItemAsync(Key, tokenModel);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _localStorage.RemoveItemAsync(Key);
        _httpClient.DefaultRequestHeaders.Authorization = null;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}