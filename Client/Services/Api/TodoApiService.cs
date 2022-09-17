using System.Net.Http.Json;
using Application.Todos.Models;
using Domain.Todos.Queries;
using Domain.Todos.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace Client.Services.Api;

public class TodoApiService : ApiServiceBase<TodoResultDTO, TodoCommand, TodoQueryParameter>
{
    public TodoApiService(
        HttpClient httpClient,
        ISnackbar snackbar,
        NavigationManager navigation,
        AuthenticationStateProvider authenticationStateProvider
    ) : base(httpClient, snackbar, navigation, authenticationStateProvider)
    {
    }

    protected override string Resource => "Todo";

    public async Task<TodoResultDTO?> ChangeState(Guid id, TodoState state)
    {
        var command = new TodoStateCommand { State = state.DisplayValue.ToLower() };
        var response = await _httpClient.PutAsJsonAsync($"{Resource}/{id}/state", command);
        return await HandleResponse<TodoResultDTO>(response, showValidationError: true);
    }
}
