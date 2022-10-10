using Client.Components.Common;
using Client.Services;
using Domain.Todos.DTOs;
using Domain.Todos.Entities;
using Domain.Todos.Queries;
using Domain.Todos.ValueObjects;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Client.Pages;

public partial class VirtualizationTestPage : ComponentBase
{
    private ApiVirtualize<Todo, TodoResultDTO, TodoCommand, TodoQueryParameter> _virtualize = null!;

    [Inject]
    private VirtualizeStoreService<TodoResultDTO> Store { get; set; } = null!;

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        await Store.ResumeCurrentAsync();
    }

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
