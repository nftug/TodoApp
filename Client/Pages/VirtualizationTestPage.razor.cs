using Client.Components.Common;
using Client.Services;
using Client.Shared.Models;
using Domain.Todos.DTOs;
using Domain.Todos.Entities;
using Domain.Todos.Queries;
using Domain.Todos.ValueObjects;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Client.Pages;

public partial class VirtualizationTestPage : ComponentBase
{
    private List<VirtualizedItem<TodoResultDTO>> CacheList { get; set; } = new();
    private ApiVirtualize<Todo, TodoResultDTO, TodoCommand, TodoQueryParameter> _virtualize { get; set; } = null!;

    private static Color GetTodoChipColor(TodoResultDTO item)
    {
        var state = new TodoState(item.State);
        return state == TodoState.Doing
            ? Color.Tertiary
            : state == TodoState.Done
            ? Color.Success
            : Color.Primary;
    }

    private async Task OnCreateItem()
    {
        await _virtualize.RefreshDataAsync();
    }
}
