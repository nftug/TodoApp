using System.Net.Http.Json;
using Microsoft.AspNetCore.WebUtilities;
using Client.Models;

namespace Client.Services.Api;

public abstract class ApiServiceBase<TResultDTO, TCommandDTO, TQueryParameter>
    where TQueryParameter : class
{
    protected readonly HttpClient _httpClient;

    protected ApiServiceBase(HttpClient httpClient)
    {
        _httpClient = httpClient;
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

        var result = await _httpClient.GetFromJsonAsync<Pagination<TResultDTO>>(uri);
        return result;
    }

    public virtual async Task<TResultDTO?> Get(Guid id)
        => await _httpClient.GetFromJsonAsync<TResultDTO>($"{Resource}/{id}");

    public virtual async Task<TResultDTO?> Create(TCommandDTO command)
    {
        var response = await _httpClient.PostAsJsonAsync(Resource, command);
        return await response.Content.ReadFromJsonAsync<TResultDTO>();
    }

    public virtual async Task<TResultDTO?> Put(Guid id, TCommandDTO command)
    {
        var response = await _httpClient.PutAsJsonAsync($"{Resource}/{id}", command);
        return await response.Content.ReadFromJsonAsync<TResultDTO>();
    }

    public virtual async Task Delete(Guid id)
        => await _httpClient.DeleteAsync($"{Resource}/{id}");
}
