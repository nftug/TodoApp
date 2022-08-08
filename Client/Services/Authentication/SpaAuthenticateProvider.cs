using System.Net.Http.Headers;
using System.Security.Claims;
using Application.Users.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace Client.Services.Authentication;

public class SpaAuthenticateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public SpaAuthenticateProvider(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var savedToken = await _localStorage.GetItemAsync<string>("authToken");
        var userName = await _localStorage.GetItemAsync<string>("userName");
        var userId = await _localStorage.GetItemAsync<Guid>("userId");

        // トークンが見つからなければ未ログイン扱いにする
        if (string.IsNullOrWhiteSpace(savedToken))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        // HttpClientのヘッダーにトークンを加える
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);

        var claims = new List<Claim> {
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "User"));
        return new AuthenticationState(user);
    }

    public async Task MarkUserAsAuthenticated(TokenModel tokenModel)
    {
        // ローカルストレージに認証情報を保持して変更通知を行う
        await _localStorage.SetItemAsync("userName", tokenModel.UserName);
        await _localStorage.SetItemAsync("authToken", tokenModel.Token);
        await _localStorage.SetItemAsync("userId", tokenModel.UserId);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task MarkUserAsLoggedOut()
    {
        // ローカルストレージの認証情報を削除して変更通知を行う
        await _localStorage.RemoveItemAsync("userName");
        await _localStorage.RemoveItemAsync("authToken");
        await _localStorage.RemoveItemAsync("userId");
        if (_httpClient.DefaultRequestHeaders.Authorization != null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}