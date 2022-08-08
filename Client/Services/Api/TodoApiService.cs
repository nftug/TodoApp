using System.Net.Http.Json;
using Application.Todos.Models;
using Domain.Todos.Queries;
using Domain.Todos.ValueObjects;

namespace Client.Services.Api;

public class TodoApiService : ApiServiceBase<TodoResultDTO, TodoCommandDTO, TodoQueryParameter>
{
    public TodoApiService(HttpClient httpClient) : base(httpClient)
    {
    }

    protected override string Resource => "Todo";

    public static TodoCommandDTO ResultToCommand(TodoResultDTO origin)
        => new()
        {
            Id = origin.Id,
            Title = origin.Title,
            Description = origin.Description,
            StartDate = origin.StartDate,
            EndDate = origin.EndDate,
            State = origin.State.Value
        };

    public async Task<TodoResultDTO?> ChangeState(Guid id, TodoState state)
    {
        var command = new TodoStateCommand { State = state.Value };
        var response = await _httpClient.PutAsJsonAsync($"{Resource}/{id}/state", command);
        return await response.Content.ReadFromJsonAsync<TodoResultDTO>();
    }
}
