using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using MudBlazor;
using System.Net;
using Microsoft.AspNetCore.Components;
using Client.Shared.Exceptions;
using Client.Shared.Extensions;
using Domain.Shared.Models;
using Client.Services.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

namespace Client.Services.Api;

public abstract class ApiServiceBase<TResultDTO, TCommandDTO, TQueryParameter> : IApiService<TResultDTO, TCommandDTO, TQueryParameter>
{
    protected readonly HttpClient _httpClient;
    protected readonly ISnackbar _snackbar;
    protected readonly NavigationManager _navigation;
    protected readonly SpaAuthenticateProvider _authenticationStateProvider;

    protected ApiServiceBase(
        HttpClient httpClient,
        ISnackbar snackbar,
        NavigationManager navigation,
        AuthenticationStateProvider authenticationStateProvider
    )
    {
        _httpClient = httpClient;
        _snackbar = snackbar;
        _navigation = navigation;
        _authenticationStateProvider = (SpaAuthenticateProvider)authenticationStateProvider;
    }

    protected abstract string Resource { get; }

    public virtual async Task<Pagination<TResultDTO>?> GetList(TQueryParameter param, bool showValidationError = false)
    {
        var props = param!.GetType().GetProperties();
        var queries = props
             .Select(prop => new
             {
                 prop.Name,
                 Value = prop.GetValue(param)?.ToString()
             })
             .Where(field => field.Value != null)
             .ToDictionary(x => x.Name, x => x.Value);
        var uri = QueryHelpers.AddQueryString(Resource, queries);

        return await GetRequest<Pagination<TResultDTO>>(uri, showValidationError);
    }

    public virtual async Task<TResultDTO?> Get(Guid id, bool showValidationError = false)
        => await GetRequest<TResultDTO>($"{Resource}/{id}", showValidationError);

    private async Task<T?> GetRequest<T>(string uri, bool showValidationError = false)
    {
        var response = await _httpClient.GetAsync(uri);
        return await HandleResponse<T>(response, showValidationError);
    }

    public virtual async Task<TResultDTO?> Create(TCommandDTO command, bool showValidationError = false)
    {
        var response = await _httpClient.PostAsJsonAsync(Resource, command);
        return await HandleResponse<TResultDTO>(response, showValidationError);
    }

    public virtual async Task<TResultDTO?> Put(Guid id, TCommandDTO command, bool showValidationError = false)
    {
        var response = await _httpClient.PutAsJsonAsync($"{Resource}/{id}", command);
        return await HandleResponse<TResultDTO>(response, showValidationError);
    }

    public virtual async Task<TResultDTO?> Delete(Guid id, bool showValidationError = false)
    {
        var response = await _httpClient.DeleteAsync($"{Resource}/{id}");
        return await HandleResponse<TResultDTO>(response, showValidationError);
    }

    protected async Task<T?> HandleResponse<T>(HttpResponseMessage response, bool showValidationError = false)
    {
        try
        {
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch (HttpRequestException e)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    if (showValidationError) await HandleValidationError(response);
                    throw await HttpValidationErrorException.CreateAsync(response);
                case HttpStatusCode.Forbidden:
                    _snackbar.Add("アクセスに必要な権限がありません。", Severity.Warning);
                    break;
                case HttpStatusCode.Unauthorized:
                    _snackbar.Add("ログインが必要です。", Severity.Warning);
                    await _authenticationStateProvider.MarkUserAsLoggedOut();

                    var currentUri = _navigation.ToBaseRelativePath(_navigation.Uri);
                    _navigation.NavigateTo($"/login?redirect={currentUri}", false, true);
                    break;
                case HttpStatusCode.InternalServerError:
                    _snackbar.Add("サーバーエラーです。", Severity.Error);
                    break;
                default:
                    _snackbar.Add("サーバーでエラーが発生しました。", Severity.Error);
                    break;
            }
            throw e;
        }
    }

    protected async Task HandleValidationError(HttpResponseMessage response)
    {
        var errorDetails = await response.GetErrorDetailsAsync();

        if (errorDetails?.Errors == null) return;
        foreach (var error in errorDetails.Errors)
        {
            _snackbar.Add($"{error.Key}で検証エラーが発生しました：<br>" +
                string.Join("<br>", error.Value), Severity.Warning);
        }
    }
}
