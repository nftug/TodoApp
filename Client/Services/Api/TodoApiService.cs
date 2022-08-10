using System.Net.Http.Json;
using Application.Todos.Models;
using Domain.Todos.Queries;
using Domain.Todos.ValueObjects;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Client.Services.Api;

public class TodoApiService : ApiServiceBase<TodoResultDTO, TodoCommandDTO, TodoQueryParameter>
{
    public TodoApiService(HttpClient httpClient, ISnackbar snackbar, NavigationManager navigation)
        : base(httpClient, snackbar, navigation)
    {
    }

    protected override string Resource => "Todo";

    public async Task<TodoResultDTO?> ChangeState(Guid id, TodoState state)
    {
        var command = new TodoStateCommand { State = state.DisplayValue.ToLower() };
        var response = await _httpClient.PutAsJsonAsync($"{Resource}/{id}/state", command);
        return await HandleResponse(response);
    }
}
