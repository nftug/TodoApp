using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using Client.Models;
using MudBlazor;
using System.Net;
using Microsoft.AspNetCore.Components;

namespace Client.Services.Api;

public abstract class ApiServiceBase<TResultDTO, TCommandDTO, TQueryParameter>
    where TQueryParameter : class
{
    protected readonly HttpClient _httpClient;
    protected readonly ISnackbar _snackbar;
    protected readonly NavigationManager _navigation;

    protected ApiServiceBase(HttpClient httpClient, ISnackbar snackbar, NavigationManager navigation)
    {
        _httpClient = httpClient;
        _snackbar = snackbar;
        _navigation = navigation;
    }

    protected abstract string Resource { get; }

    public virtual async Task<Pagination<TResultDTO>?> GetList(TQueryParameter param)
    {
        var props = param.GetType().GetProperties();
        var queries = props
             .Select(prop => new
             {
                 prop.Name,
                 Value = prop.GetValue(param)?.ToString()
             })
             .Where(field => field.Value != null)
             .ToDictionary(x => x.Name, x => x.Value);
        var uri = QueryHelpers.AddQueryString(Resource, queries);

        try
        {
            return await _httpClient.GetFromJsonAsync<Pagination<TResultDTO>>(uri);
        }
        catch (HttpRequestException e)
        {
            HandleHttpRequestException(e);
            throw e;
        }
    }

    public virtual async Task<TResultDTO?> Get(Guid id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<TResultDTO>($"{Resource}/{id}");
        }
        catch (HttpRequestException e)
        {
            HandleHttpRequestException(e);
            throw e;
        }
    }

    public virtual async Task<TResultDTO?> Create(TCommandDTO command)
    {
        var response = await _httpClient.PostAsJsonAsync(Resource, command);
        return await HandleResponse(response);
    }

    public virtual async Task<TResultDTO?> Put(Guid id, TCommandDTO command)
    {
        var response = await _httpClient.PutAsJsonAsync($"{Resource}/{id}", command);
        return await HandleResponse(response);
    }

    public virtual async Task<TResultDTO?> Delete(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"{Resource}/{id}");
        return await HandleResponse(response);
    }

    protected virtual async Task<TResultDTO?> HandleResponse(HttpResponseMessage response)
    {
        try
        {
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResultDTO>();
        }
        catch (HttpRequestException e)
        {
            HandleHttpRequestException(e);
            throw e;
        }
    }

    protected void HandleHttpRequestException(HttpRequestException exception)
    {
        switch (exception.StatusCode)
        {
            case HttpStatusCode.BadRequest:
                _snackbar.Add("この操作を行う権限がありません。", Severity.Error);
                break;
            case HttpStatusCode.Unauthorized:
                _snackbar.Add("ログインが必要です。", Severity.Warning);
                var currentUri = _navigation.ToBaseRelativePath(_navigation.Uri);
                _navigation.NavigateTo($"/login?redirect={currentUri}", false, true);
                break;
            case HttpStatusCode.InternalServerError:
                _snackbar.Add("サーバーエラーです。", Severity.Error);
                break;
        }
    }
}
